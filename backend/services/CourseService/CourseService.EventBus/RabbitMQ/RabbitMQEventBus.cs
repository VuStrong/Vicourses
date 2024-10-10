using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CourseService.EventBus.RabbitMQ
{
    internal class RabbitMQEventBus : IEventBus, IHostedService, IDisposable
    {
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IModel? _consumerChannel;

        private readonly Dictionary<Type, RabbitMQPublishEventOptions> _publishEventOptions;
        private readonly Dictionary<Type, RabbitMQConsumeEventOptions> _consumeEventOptions;

        public RabbitMQEventBus(
            IServiceProvider serviceProvider,
            ILogger<RabbitMQEventBus> logger,
            RabbitMQEventBusConfigurator rabbitmqConfigurator)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _publishEventOptions = rabbitmqConfigurator.PublishEventOptions;
            _consumeEventOptions = rabbitmqConfigurator.ConsumeEventOptions;
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            if (_connection == null || !_connection.IsOpen) return;

            using var channel = _connection.CreateModel();

            var message = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            var body = Encoding.UTF8.GetBytes(message);
            
            var eventType = typeof(T);

            if (!_publishEventOptions.TryGetValue(eventType, out var publishOptions)) return;

            var exchangeOptions = publishOptions.ExchangeOptions;

            if (!publishOptions.ExcludeExchange && !string.IsNullOrEmpty(exchangeOptions.ExchangeName))
            {
                channel.ExchangeDeclare(
                    exchange: exchangeOptions.ExchangeName, 
                    type: exchangeOptions.ExchangeType, 
                    durable: exchangeOptions.Durable,
                    autoDelete: exchangeOptions.AutoDelete);
            }

            channel.BasicPublish(
                exchange: exchangeOptions.ExchangeName,
                routingKey: publishOptions.RoutingKey,
                basicProperties: null,
                body: body);
        }

        private void StartConsume()
        {
            try
            {
                _connection = _serviceProvider.GetRequiredService<IConnection>();

                _consumerChannel = _connection.CreateModel();

                foreach (KeyValuePair<Type, RabbitMQConsumeEventOptions> entry in _consumeEventOptions)
                {
                    var eventType = entry.Key;
                    var consumeOptions = entry.Value;
                    var exchangeOptions = consumeOptions.ExchangeOptions;
                    var queueOptions = consumeOptions.QueueOptions;

                    if (!consumeOptions.ExcludeExchange && !string.IsNullOrEmpty(exchangeOptions.ExchangeName))
                    {
                        _consumerChannel.ExchangeDeclare(
                            exchange: exchangeOptions.ExchangeName,
                            type: exchangeOptions.ExchangeType,
                            durable: exchangeOptions.Durable,
                            autoDelete: exchangeOptions.AutoDelete);
                    }

                    var queueName = _consumerChannel.QueueDeclare(
                        queue: queueOptions.QueueName,
                        durable: queueOptions.Durable,
                        autoDelete: queueOptions.AutoDelete,
                        exclusive: queueOptions.Exclusive).QueueName;

                    _consumerChannel.QueueBind(
                        queue: queueName,
                        exchange: exchangeOptions.ExchangeName,
                        routingKey: consumeOptions.RoutingKey);

                    var eventConsumer = new EventConsumer(
                        eventType,
                        _consumerChannel,
                        _serviceProvider,
                        _logger
                    );
                    eventConsumer.AutoAck = consumeOptions.AutoAck;

                    var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                    consumer.Received += eventConsumer.Consume;

                    _consumerChannel.BasicConsume(
                        queue: queueName,
                        autoAck: consumeOptions.AutoAck,
                        consumer: consumer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while starting RabbitMQ connection: {msg}", ex.Message);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(() =>
            {
                StartConsume();
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }
      
        private class EventConsumer
        {
            private readonly Type _eventType;
            private readonly IModel _channel;
            private readonly IServiceProvider _serviceProvider;
            private readonly ILogger<RabbitMQEventBus> _logger;

            public bool AutoAck { get; set; } = true;

            public EventConsumer(
                Type eventType,
                IModel channel,
                IServiceProvider serviceProvider,
                ILogger<RabbitMQEventBus> logger)
            {
                _eventType = eventType;
                _channel = channel;
                _serviceProvider = serviceProvider;
                _logger = logger;
            }

            public async Task Consume(object? sender, BasicDeliverEventArgs e)
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                try
                {
                    await ProcessEvent(message);

                    if (!AutoAck)
                    {
                        _channel.BasicAck(e.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error when process event {e.Exchange} : {ex.Message}");
                }
            }

            private async Task ProcessEvent(string message)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var @event = JsonSerializer.Deserialize(
                    message,
                    _eventType,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    }) as IntegrationEvent;

                if (@event == null) return;

                var handlers = scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(_eventType.Name);

                foreach (var handler in handlers)
                {
                    await handler.Handle(@event);
                }
            }
        }
    }
}
