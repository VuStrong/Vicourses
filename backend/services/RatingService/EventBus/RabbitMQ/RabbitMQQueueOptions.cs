namespace EventBus.RabbitMQ
{
    public class RabbitMQQueueOptions
    {
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public string QueueName { get; set; } = string.Empty;
    }
}
