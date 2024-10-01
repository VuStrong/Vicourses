using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lession;

namespace CourseService.Application.DomainEventHandlers.Lession
{
    public class LessionDeletedDomainEventHandler : IDomainEventHandler<LessionDeletedDomainEvent>
    {
        private readonly IQuizRepository _quizRepository;

        public LessionDeletedDomainEventHandler(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task Handle(LessionDeletedDomainEvent @event)
        {
            if (@event.Lession.Type == Domain.Enums.LessionType.Quiz)
            {
                await _quizRepository.DeleteByLessionIdAsync(@event.Lession.Id);
            }

            // Todo: remove lession's video in storage service
        }
    }
}
