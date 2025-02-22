using EventBus;
using RatingService.API.Application.Dtos;

namespace RatingService.API.Application.IntegrationEvents.Course
{
    public class CourseInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public PublicUserDto User { get; set; } = null!;
    }
}
