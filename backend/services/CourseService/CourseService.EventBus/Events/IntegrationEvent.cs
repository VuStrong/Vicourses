namespace CourseService.EventBus.Events
{
    public abstract class IntegrationEvent
    {
        public abstract string ExchangeName
        {
            get;
        }
    }
}
