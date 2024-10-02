using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Section;

namespace CourseService.Application.DomainEventHandlers.Section
{
    public class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
    {
        private readonly ILessionRepository _lessionRepository;
        private readonly IQuizRepository _quizRepository;

        public SectionDeletedDomainEventHandler(
            ILessionRepository lessionRepository,
            IQuizRepository quizRepository)
        {
            _lessionRepository = lessionRepository;
            _quizRepository = quizRepository;
        }

        public async Task Handle(SectionDeletedDomainEvent @event)
        {
            var lessions = await _lessionRepository.FindBySectionIdAsync(@event.Section.Id);
            var videoFiles = lessions.Where(l => l.Video != null).Select(l => l.Video);
            var quizLessionIds = lessions.Where(l => l.Type == Domain.Enums.LessionType.Quiz).Select(l => l.Id);

            await _lessionRepository.DeleteBySectionIdAsync(@event.Section.Id);
            
            if (quizLessionIds.Any())
                await _quizRepository.DeleteByLessionIdsAsync(quizLessionIds);

            // todo: Send files to storage service to remove
        }
    }
}
