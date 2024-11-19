using EventBus;

namespace StatisticsService.API.Application.IntegrationEvents.Course
{
    public class CoursePublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public UserInCourseIntegrationEvent User { get; set; } = null!;
    }

    public class UserInCourseIntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
