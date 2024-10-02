using CourseService.EventBus.Events;

namespace CourseService.Application.IntegrationEvents.User
{
    public class UserInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public override string ExchangeName
        {
            get { return "user.info.updated"; }
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
    }
}
