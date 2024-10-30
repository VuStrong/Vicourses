namespace EventBus.RabbitMQ
{
    public static class RabbitMQExchangeType
    {
        public const string Fanout = "fanout";
        public const string Direct = "direct";
        public const string Topic = "topic";
        public const string Header = "header";
    }
}
