using CourseService.EventBus.Events;

namespace CourseService.Application.IntegrationEvents.Course
{
    internal class CourseUnpublishedIntegrationEvent : IntegrationEvent
    {
        public override string ExchangeName
        {
            get { return "course.unpublished"; }
        }

        public string Id { get; set; } = string.Empty;
    }
}
