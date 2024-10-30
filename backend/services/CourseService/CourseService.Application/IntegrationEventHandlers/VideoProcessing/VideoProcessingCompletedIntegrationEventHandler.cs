using CourseService.Application.Exceptions;
using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Domain.Contracts;
using EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.VideoProcessing
{
    public class VideoProcessingCompletedIntegrationEventHandler : IIntegrationEventHandler<VideoProcessingCompletedIntegrationEvent>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILogger<VideoProcessingCompletedIntegrationEventHandler> _logger;

        public VideoProcessingCompletedIntegrationEventHandler(
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            ILogger<VideoProcessingCompletedIntegrationEventHandler> logger)
        {
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
            _logger = logger;
        }

        public async Task Handle(VideoProcessingCompletedIntegrationEvent @event)
        {
            _logger.LogInformation($"CourseService handle VideoProcessingCompletedIntegrationEvent for {@event.Entity}: {@event.EntityId}");

            if (@event.Entity == "course")
            {
                var course = await _courseRepository.FindOneAsync(@event.EntityId) ?? throw new CourseNotFoundException(@event.EntityId);

                course.SetPreviewVideoStatusCompleted(@event.StreamFileUrl, @event.Duration);

                await _courseRepository.UpdateAsync(course);
            }
            else if (@event.Entity == "lesson")
            {
                var lesson = await _lessonRepository.FindOneAsync(@event.EntityId) ?? throw new LessonNotFoundException(@event.EntityId);

                lesson.SetVideoStatusCompleted(@event.StreamFileUrl, @event.Duration);

                await _lessonRepository.UpdateAsync(lesson);
            }
        }
    }
}