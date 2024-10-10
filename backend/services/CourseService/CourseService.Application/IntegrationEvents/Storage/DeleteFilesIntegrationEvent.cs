using CourseService.EventBus;

namespace CourseService.Application.IntegrationEvents.Storage
{
    public class DeleteFilesIntegrationEvent : IntegrationEvent
    {
        public List<string> FileIds { get; set; } = [];
    }
}