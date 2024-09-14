using CourseService.EventBus.Abstracts;
using CourseService.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CourseService.EventBus
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly Dictionary<string, Type> _events;
        private readonly IConnection _connection = null!;
        private readonly IModel _channel = null!;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQEventBus(
            string connectionString, 
            IServiceScopeFactory serviceScopeFactory,
            ILogger<RabbitMQEventBus> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _handlers = [];
            _events = [];

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not connect to RabbitMQ: {msg}", ex.Message);
            }
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            if (_connection == null || _channel == null || !_connection.IsOpen) return;

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);
            var exchangeName = @event.ExchangeName;

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true);

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: "",
                basicProperties: null,
                body: body);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            if (_connection == null || _channel == null || !_connection.IsOpen) return;

            var @event = Activator.CreateInstance(typeof(T));
            var exchangeName = @event != null ? ((T)@event).ExchangeName : "";
            var eventType = typeof(T);
            var handlerType = typeof(TH);

            if (!_events.ContainsKey(exchangeName))
            {
                _events.Add(exchangeName, eventType);
            }

            if (!_handlers.ContainsKey(exchangeName))
            {
                _handlers.Add(exchangeName, new List<Type>());
            }

            if (_handlers[exchangeName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is regisered for event '{eventType.Name}'", nameof(handlerType));
            }

            _handlers[exchangeName].Add(handlerType);

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true);
            var queueName = _channel.QueueDeclare(
                queue: $"courses_{eventType.Name}", 
                durable: true,
                autoDelete: false,
                exclusive: false).QueueName;
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
        }

        private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            ProcessEvent(e.Exchange, message);
        }

        private async Task ProcessEvent(string exchangeName, string message)
        {
            if (!_events.ContainsKey(exchangeName) || !_handlers.ContainsKey(exchangeName)) return;

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                
                var handlerTypes = _handlers[exchangeName];
                var eventType = _events[exchangeName];

                var @event = JsonSerializer.Deserialize(
                    message,
                    eventType,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                foreach (var handlerType in handlerTypes)
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);

                    if (handler == null) continue;

                    var method = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);

                    var task = method!.Invoke(handler, new object[] { @event! }) as Task;

                    await task!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when process event from RabbitMQ: {msg}", ex.Message);
            }
        }

        public void Dispose()
        {
            if (_connection != null && _channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
