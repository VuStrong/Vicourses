using AutoMapper;
using EventBus;
using SearchService.API.Application.IntegrationEvents.Course;
using SearchService.API.Application.Interfaces;

namespace SearchService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseInfoUpdatedIntegrationEvent>
    {
        private readonly ICoursesCommandService _coursesCommandService;
        private readonly ILogger<CourseInfoUpdatedIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;

        public CourseInfoUpdatedIntegrationEventHandler(
            ICoursesCommandService coursesCommandService,
            ILogger<CourseInfoUpdatedIntegrationEventHandler> logger,
            IMapper mapper)
        {
            _coursesCommandService = coursesCommandService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(CourseInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Search Service] Handle CourseInfoUpdatedIntegrationEvent: {@event.Id}");

            var course = _mapper.Map<Models.Course>(@event);

            await _coursesCommandService.InsertOrUpdateCourseAsync(course);
        }
    }
}
