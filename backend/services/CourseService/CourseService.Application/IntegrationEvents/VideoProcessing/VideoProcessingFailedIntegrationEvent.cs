using CourseService.EventBus;

namespace CourseService.Application.IntegrationEvents.VideoProcessing
{
    public class VideoProcessingFailedIntegrationEvent : IntegrationEvent
    {
        public string Entity { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
    }
}