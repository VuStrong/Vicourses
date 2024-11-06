using EventBus;
using WishlistService.API.Application.Dtos;

namespace WishlistService.API.Application.IntegrationEvents.Course
{
    public class CoursePublishedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleCleaned { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public string? ThumbnailUrl { get; set; }
        public PublicUserDto User { get; set; } = null!;
    }
}
