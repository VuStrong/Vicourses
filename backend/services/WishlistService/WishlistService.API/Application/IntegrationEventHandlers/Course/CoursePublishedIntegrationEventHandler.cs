using AutoMapper;
using EventBus;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Application.Services;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;

        public CoursePublishedIntegrationEventHandler(
            ICourseService courseService,
            IMapper mapper,
            ILogger<CoursePublishedIntegrationEventHandler> logger)
        {
            _courseService = courseService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle CoursePublishedIntegrationEvent: {@event.Id}");

            var course = _mapper.Map<Models.Course>(@event);

            await _courseService.AddOrUpdateCourseAsync(course);
        }
    }
}
