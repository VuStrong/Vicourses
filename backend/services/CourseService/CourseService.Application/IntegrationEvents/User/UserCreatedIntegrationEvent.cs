using CourseService.EventBus.Events;

namespace CourseService.Application.IntegrationEvents.User
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public override string ExchangeName
        {
            get { return "user.created"; }
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
