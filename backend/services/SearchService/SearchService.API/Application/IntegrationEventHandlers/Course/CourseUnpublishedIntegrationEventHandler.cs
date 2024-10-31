using EventBus;
using SearchService.API.Application.IntegrationEvents.Course;
using SearchService.API.Application.Interfaces;

namespace SearchService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseUnpublishedIntegrationEventHandler : IIntegrationEventHandler<CourseUnpublishedIntegrationEvent>
    {
        private readonly ICoursesCommandService _coursesCommandService;
        private readonly ILogger<CourseUnpublishedIntegrationEventHandler> _logger;

        public CourseUnpublishedIntegrationEventHandler(
            ICoursesCommandService coursesCommandService,
            ILogger<CourseUnpublishedIntegrationEventHandler> logger)
        {
            _coursesCommandService = coursesCommandService;
            _logger = logger;
        }

        public async Task Handle(CourseUnpublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Search Service] Handle CourseUnpublishedIntegrationEvent: {@event.Id}");

            await _coursesCommandService.DeleteCourseAsync(@event.Id);
        }
    }
}
