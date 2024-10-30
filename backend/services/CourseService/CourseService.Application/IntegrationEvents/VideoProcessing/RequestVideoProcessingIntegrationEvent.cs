using EventBus;

namespace CourseService.Application.IntegrationEvents.VideoProcessing
{
    public class RequestVideoProcessingIntegrationEvent : IntegrationEvent
    {
        public string FileId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
    }
}