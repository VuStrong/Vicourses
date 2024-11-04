using EventBus;

namespace RatingService.API.Application.IntegrationEvents.Rating
{
    public class CourseRatingUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = null!;
        public decimal AvgRating { get; set; }
    }
}
