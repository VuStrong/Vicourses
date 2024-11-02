using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Section;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Section
{
    public class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IEventBus _eventBus;

        public SectionDeletedDomainEventHandler(
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IEventBus eventBus)
        {
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(SectionDeletedDomainEvent @event)
        {
            var lessons = await _lessonRepository.FindBySectionIdAsync(@event.Section.Id);
            var quizLessonIds = lessons.Where(l => l.Type == LessonType.Quiz).Select(l => l.Id);

            await _lessonRepository.DeleteBySectionIdAsync(@event.Section.Id);
            
            if (quizLessonIds.Any())
                await _quizRepository.DeleteByLessonIdsAsync(quizLessonIds);

            var videoFiles = lessons.Where(l => l.Video != null).Select(l => l.Video?.FileId ?? "").ToList();
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
