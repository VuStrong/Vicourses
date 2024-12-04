using EventBus;

namespace WishlistService.API.Application.IntegrationEvents.Course
{
    public class UserUnenrolledIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; } = string.Empty;
        public CourseInUserUnenrolledIntegrationEvent Course { get; set; } = null!;
    }

    public class CourseInUserUnenrolledIntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
