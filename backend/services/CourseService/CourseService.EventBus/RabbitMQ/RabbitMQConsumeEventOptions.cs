namespace CourseService.EventBus.RabbitMQ
{
    public class RabbitMQConsumeEventOptions
    {
        public RabbitMQExchangeOptions ExchangeOptions { get; set; } = new();
        public RabbitMQQueueOptions QueueOptions { get; set; } = new();
        public string RoutingKey { get; set; } = string.Empty;
        public bool ExcludeExchange { get; set; } = false;
        public bool AutoAck { get; set; } = true;
    }
}
