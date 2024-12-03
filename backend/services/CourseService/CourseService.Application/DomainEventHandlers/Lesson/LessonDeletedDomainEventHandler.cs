using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Lesson
{
    internal class LessonDeletedDomainEventHandler : IDomainEventHandler<LessonDeletedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public LessonDeletedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(LessonDeletedDomainEvent @event)
        {
            var videoFileId = @event.Lesson.Video?.FileId;

            if (videoFileId != null)
            {
                _eventBus.Publish(new DeleteFilesIntegrationEvent
                {
                    FileIds = [videoFileId],
                });
            }

            return Task.CompletedTask;
        }
    }
}
