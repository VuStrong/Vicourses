using EventBus;

namespace CourseService.Application.IntegrationEvents.VideoProcessing
{
    public class VideoProcessingCompletedIntegrationEvent : IntegrationEvent
    {
        public string StreamFileUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Entity { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
    }
}