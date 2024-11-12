using EventBus;

namespace DiscountService.API.Application.IntegrationEvents.Course
{
    public class CourseInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public UserInCourseIntegrationEvent User { get; set; } = null!;
        public decimal Price { get; set; }
    }

    public class UserInCourseIntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
