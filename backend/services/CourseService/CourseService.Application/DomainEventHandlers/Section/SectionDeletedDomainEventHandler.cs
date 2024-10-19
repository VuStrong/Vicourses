using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Section;

namespace CourseService.Application.DomainEventHandlers.Section
{
    public class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;

        public SectionDeletedDomainEventHandler(
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository)
        {
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
        }

        public async Task Handle(SectionDeletedDomainEvent @event)
        {
            var lessons = await _lessonRepository.FindBySectionIdAsync(@event.Section.Id);
            var videoFiles = lessons.Where(l => l.Video != null).Select(l => l.Video);
            var quizLessonIds = lessons.Where(l => l.Type == Domain.Enums.LessonType.Quiz).Select(l => l.Id);

            await _lessonRepository.DeleteBySectionIdAsync(@event.Section.Id);
            
            if (quizLessonIds.Any())
                await _quizRepository.DeleteByLessonIdsAsync(quizLessonIds);

            // todo: Send files to storage service to remove
        }
    }
}
