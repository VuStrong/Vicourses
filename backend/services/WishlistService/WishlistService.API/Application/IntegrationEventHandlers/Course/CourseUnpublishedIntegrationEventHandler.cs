using EventBus;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Application.Services;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseUnpublishedIntegrationEventHandler : IIntegrationEventHandler<CourseUnpublishedIntegrationEvent>
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseUnpublishedIntegrationEventHandler> _logger;

        public CourseUnpublishedIntegrationEventHandler(
            ICourseService courseService,
            ILogger<CourseUnpublishedIntegrationEventHandler> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        public async Task Handle(CourseUnpublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle CourseUnpublishedIntegrationEvent: {@event.Id}");

            await _courseService.UnpublishCourseAsync(@event.Id);
        }
    }
}
