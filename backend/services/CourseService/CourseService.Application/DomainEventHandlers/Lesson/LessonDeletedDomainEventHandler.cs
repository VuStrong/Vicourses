using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Contracts;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.EventBus;

namespace CourseService.Application.DomainEventHandlers.Lesson
{
    public class LessonDeletedDomainEventHandler : IDomainEventHandler<LessonDeletedDomainEvent>
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IEventBus _eventBus;

        public LessonDeletedDomainEventHandler(IQuizRepository quizRepository, IEventBus eventBus)
        {
            _quizRepository = quizRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(LessonDeletedDomainEvent @event)
        {
            if (@event.Lesson.Type == Domain.Enums.LessonType.Quiz)
            {
                await _quizRepository.DeleteByLessonIdAsync(@event.Lesson.Id);
            }
            else
            {
                var videoFileId = @event.Lesson.Video?.FileId;

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
