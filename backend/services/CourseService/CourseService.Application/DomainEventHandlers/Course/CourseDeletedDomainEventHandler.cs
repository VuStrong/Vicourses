using CourseService.Application.IntegrationEvents.Storage;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    internal class CourseDeletedDomainEventHandler : IDomainEventHandler<CourseDeletedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public CourseDeletedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(CourseDeletedDomainEvent @event)
        {
            var course = @event.Course;
            List<string> fileIds = [];

            if (course.Thumbnail != null)
            {
                fileIds.Add(course.Thumbnail.FileId);
            }
            if (course.PreviewVideo != null)
            {
                fileIds.Add(course.PreviewVideo.FileId);
            }

            if (fileIds.Count > 0)
            {
                _eventBus.Publish(new DeleteFilesIntegrationEvent
                {
                    FileIds = fileIds
                });
            }

            return Task.CompletedTask;
        }
    }
}
