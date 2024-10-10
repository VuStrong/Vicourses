namespace CourseService.EventBus.RabbitMQ
{
    public class RabbitMQPublishEventOptions
    {
        public RabbitMQExchangeOptions ExchangeOptions { get; set; } = new();
        public string RoutingKey { get; set; } = string.Empty;
        public bool ExcludeExchange { get; set; } = false;
    }
}
