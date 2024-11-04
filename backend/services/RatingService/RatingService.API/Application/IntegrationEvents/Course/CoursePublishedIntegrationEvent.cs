using EventBus;
using RatingService.API.Application.Dtos;

namespace RatingService.API.Application.IntegrationEvents.Course
{
    public class CoursePublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public PublicUserDto User { get; set; } = null!;
    }
}
