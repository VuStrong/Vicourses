using AutoMapper;
using EventBus;
using WishlistService.API.Application.IntegrationEvents.Course;
using WishlistService.API.Application.Services;

namespace WishlistService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseInfoUpdatedIntegrationEvent>
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseInfoUpdatedIntegrationEventHandler> _logger;

        public CourseInfoUpdatedIntegrationEventHandler(
            ICourseService courseService,
            IMapper mapper,
            ILogger<CourseInfoUpdatedIntegrationEventHandler> logger)
        {
            _courseService = courseService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(CourseInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Wishlist Service] Handle CourseInfoUpdatedIntegrationEvent: {@event.Id}");

            var course = _mapper.Map<Models.Course>(@event);

            await _courseService.AddOrUpdateCourseAsync(course);
        }
    }
}
