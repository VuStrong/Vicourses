using EventBus;

namespace CourseService.Application.IntegrationEvents.Rating
{
    public class CourseRatingUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public decimal AvgRating { get; set; }
    }
}