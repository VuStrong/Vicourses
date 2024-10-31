namespace EventBus.RabbitMQ
{
    public class RabbitMQEventBusConfigurator
    {
        public string UriString { get; set; } = string.Empty;

        internal Dictionary<Type, RabbitMQPublishEventOptions> PublishEventOptions { get; } = [];
        internal Dictionary<Type, RabbitMQConsumeEventOptions> ConsumeEventOptions { get; } = [];

        public void ConfigurePublish<T>(Action<RabbitMQPublishEventOptions>? config = null) where T : IntegrationEvent
        {
            var publishOptions = new RabbitMQPublishEventOptions();

            config?.Invoke(publishOptions);

            var eventType = typeof(T);

            PublishEventOptions.TryAdd(eventType, publishOptions);
        }

        public void ConfigureConsume<T>(Action<RabbitMQConsumeEventOptions>? config = null) where T : IntegrationEvent
        {
            var consumeOptions = new RabbitMQConsumeEventOptions();

            config?.Invoke(consumeOptions);

            var eventType = typeof(T);

            ConsumeEventOptions.TryAdd(eventType, consumeOptions);
        }
    }
}
