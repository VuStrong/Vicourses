using EventBus;

namespace RatingService.API.Application.IntegrationEvents.Course
{
    public class UserEnrolledIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; } = string.Empty;
        public CourseInUserEnrolledIntegrationEvent Course { get; set; } = null!;
        public DateOnly EnrolledAt { get; set; }
    }

    public class CourseInUserEnrolledIntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
