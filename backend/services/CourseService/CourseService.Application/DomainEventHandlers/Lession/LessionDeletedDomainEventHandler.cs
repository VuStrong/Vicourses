using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lession;
using CourseService.EventBus;

namespace CourseService.Application.DomainEventHandlers.Lession
{
    public class LessionDeletedDomainEventHandler : IDomainEventHandler<LessionDeletedDomainEvent>
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IEventBus _eventBus;

        public LessionDeletedDomainEventHandler(IQuizRepository quizRepository, IEventBus eventBus)
        {
            _quizRepository = quizRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(LessionDeletedDomainEvent @event)
        {
            if (@event.Lession.Type == Domain.Enums.LessionType.Quiz)
            {
                await _quizRepository.DeleteByLessionIdAsync(@event.Lession.Id);
            }
            else
            {
                var videoFileId = @event.Lession.Video?.FileId;

                if (videoFileId != null)
                {
                    _eventBus.Publish(new DeleteFilesIntegrationEvent
                    {
                        FileIds = [videoFileId],
                    });
                }
            }
        }
    }
}
