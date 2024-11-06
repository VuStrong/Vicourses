using EventBus;

namespace WishlistService.API.Application.IntegrationEvents.Course
{
    public class UserEnrolledIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; } = string.Empty;
        public CourseInUserEnrolledIntegrationEvent Course { get; set; } = null!;
    }

    public class CourseInUserEnrolledIntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
