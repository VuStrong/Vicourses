namespace EventBus.RabbitMQ
{
    public class RabbitMQExchangeOptions
    {
        public string ExchangeType { get; set; } = RabbitMQExchangeType.Fanout;
        public string ExchangeName { get; set; } = string.Empty;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
    }
}
