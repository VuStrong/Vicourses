using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseDeletedDomainEventHandler : IDomainEventHandler<CourseDeletedDomainEvent>
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IEventBus _eventBus;

        public CourseDeletedDomainEventHandler(
            ISectionRepository sectionRepository,
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IEventBus eventBus)
        {
            _sectionRepository = sectionRepository;
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(CourseDeletedDomainEvent @event)
        {
            var lessons = await _lessonRepository.FindByCourseIdAsync(@event.Course.Id);
            var quizLessonIds = lessons.Where(l => l.Type == Domain.Enums.LessonType.Quiz).Select(l => l.Id);

            await _sectionRepository.DeleteByCourseIdAsync(@event.Course.Id);

            await _lessonRepository.DeleteByCourseIdAsync(@event.Course.Id);

            if (quizLessonIds.Any()) 
                await _quizRepository.DeleteByLessonIdsAsync(quizLessonIds);

            var videoFiles = lessons.Where(l => l.Video != null).Select(l => l.Video?.FileId ?? "").ToList();
            if (@event.Course.PreviewVideo != null)
            {
                videoFiles.Add(@event.Course.PreviewVideo.FileId);
            }

            if (videoFiles.Count > 0)
            {
                _eventBus.Publish(new DeleteFilesIntegrationEvent
                {
                    FileIds = videoFiles
                });
            }
        }
    }
}
