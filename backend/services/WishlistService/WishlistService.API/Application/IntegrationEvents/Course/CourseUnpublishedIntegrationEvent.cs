using EventBus;

namespace WishlistService.API.Application.IntegrationEvents.Course
{
    public class CourseUnpublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
