using EventBus;

namespace PaymentService.API.Application.IntegrationEvents.Course
{
    public class CourseInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public UserInCourseIntegrationEvent User { get; set; } = null!;
    }
}
