using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CourseThumbnailUpdatedDomainEventHandler : IDomainEventHandler<CourseThumbnailUpdatedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public CourseThumbnailUpdatedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(CourseThumbnailUpdatedDomainEvent @event)
        {
            if (@event.OldThumbnail != null && @event.Course.Thumbnail?.FileId != @event.OldThumbnail.FileId)
            {
                _eventBus.Publish(new DeleteFilesIntegrationEvent
                {
                    FileIds = [@event.OldThumbnail.FileId]
                });
            }

            return Task.CompletedTask;
        }
    }
}