using AutoMapper;
using EventBus;
using SearchService.API.Application.IntegrationEvents.Course;
using SearchService.API.Application.Interfaces;

namespace SearchService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly ICoursesCommandService _coursesCommandService;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;

        public CoursePublishedIntegrationEventHandler(
            ICoursesCommandService coursesCommandService,
            ILogger<CoursePublishedIntegrationEventHandler> logger,
            IMapper mapper)
        {
            _coursesCommandService = coursesCommandService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Search Service] Handle CoursePublishedIntegrationEvent: {@event.Id}");

            var course = _mapper.Map<Models.Course>(@event);

            await _coursesCommandService.InsertOrUpdateCourseAsync(course);
        }
    }
}
