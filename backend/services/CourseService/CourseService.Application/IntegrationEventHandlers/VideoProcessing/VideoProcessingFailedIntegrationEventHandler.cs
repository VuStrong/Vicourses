using CourseService.Application.Exceptions;
using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Domain.Contracts;
using CourseService.EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.VideoProcessing
{
    public class VideoProcessingFailedIntegrationEventHandler : IIntegrationEventHandler<VideoProcessingFailedIntegrationEvent>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILogger<VideoProcessingFailedIntegrationEventHandler> _logger;

        public VideoProcessingFailedIntegrationEventHandler(
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            ILogger<VideoProcessingFailedIntegrationEventHandler> logger)
        {
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _logger = logger;
        }

        public async Task Handle(VideoProcessingFailedIntegrationEvent @event)
        {
            _logger.LogInformation($"CourseService handle VideoProcessingFailedIntegrationEvent for {@event.Entity}: {@event.EntityId}");

            if (@event.Entity == "course")
            {
                var course = await _courseRepository.FindOneAsync(@event.EntityId) ?? throw new CourseNotFoundException(@event.EntityId);

                course.SetPreviewVideoStatusFailed();

                await _courseRepository.UpdateAsync(course);
            }
            else if (@event.Entity == "lesson")
            {
                var lesson = await _lessonRepository.FindOneAsync(@event.EntityId) ?? throw new LessonNotFoundException(@event.EntityId);

                lesson.SetVideoStatusFailed();

                await _lessonRepository.UpdateAsync(lesson);
            }
        }
    }
}