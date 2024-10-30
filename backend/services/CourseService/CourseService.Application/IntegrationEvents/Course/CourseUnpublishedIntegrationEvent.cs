using EventBus;

namespace CourseService.Application.IntegrationEvents.Course
{
    internal class CourseUnpublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
    }
}
