using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace EventBus.RabbitMQ
{
    internal class RabbitMQEventBus : IEventBus, IHostedService, IDisposable
    {
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQEventBusConfigurator _rabbitmqConfigurator;
        private IConnection? _connection;
        private IModel? _consumerChannel;

        private readonly Dictionary<Type, RabbitMQPublishEventOptions> _publishEventOptions;
        private readonly Dictionary<Type, RabbitMQConsumeEventOptions> _consumeEventOptions;

        private static readonly JsonSerializerOptions _writeJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private static readonly JsonSerializerOptions _readJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public RabbitMQEventBus(
            IServiceProvider serviceProvider,
            ILogger<RabbitMQEventBus> logger,
            RabbitMQEventBusConfigurator rabbitmqConfigurator)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _rabbitmqConfigurator = rabbitmqConfigurator;
            _publishEventOptions = rabbitmqConfigurator.PublishEventOptions;
            _consumeEventOptions = rabbitmqConfigurator.ConsumeEventOptions;
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            if (_connection == null || !_connection.IsOpen) return;

            using var channel = _connection.CreateModel();

            var message = JsonSerializer.Serialize(@event, _writeJsonOptions);
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
            if (_connection == null || !_connection.IsOpen) return;

            try
            {
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
                        _readJsonOptions,
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
                _logger.LogError("Error consuming RabbitMQ messages: {msg}", ex.Message);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _connection = _serviceProvider.GetRequiredService<IConnection>();

                        StartConsume();

                        _logger.LogInformation("Connected to RabbitMQ");

                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error connecting to RabbitMQ: {msg}", ex.Message);

                        if (ex is not BrokerUnreachableException || _rabbitmqConfigurator.RetryDelay <= 0)
                        {
                            break;
                        }

                        await Task.Delay(TimeSpan.FromSeconds(_rabbitmqConfigurator.RetryDelay), cancellationToken);
                    }
                }
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
            private readonly JsonSerializerOptions _readJsonOptions;

            public bool AutoAck { get; set; } = true;

            public EventConsumer(
                Type eventType,
                IModel channel,
                IServiceProvider serviceProvider,
                JsonSerializerOptions readJsonOptions,
                ILogger<RabbitMQEventBus> logger)
            {
                _eventType = eventType;
                _channel = channel;
                _serviceProvider = serviceProvider;
                _readJsonOptions = readJsonOptions;
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
                    _logger.LogError($"Error when process event {_eventType.Name}: {ex.Message}");
                }
            }

            private async Task ProcessEvent(string message)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var @event = JsonSerializer.Deserialize(
                    message,
                    _eventType,
                    _readJsonOptions) as IntegrationEvent;

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
