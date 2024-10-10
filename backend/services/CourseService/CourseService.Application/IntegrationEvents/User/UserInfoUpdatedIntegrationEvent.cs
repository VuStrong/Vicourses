using CourseService.EventBus;

namespace CourseService.Application.IntegrationEvents.User
{
    public class UserInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
    }
}
