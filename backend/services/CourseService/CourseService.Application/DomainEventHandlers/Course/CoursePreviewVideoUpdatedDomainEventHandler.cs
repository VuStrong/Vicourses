using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.EventBus;

namespace CourseService.Application.DomainEventHandlers.Course
{
    public class CoursePreviewVideoUpdatedDomainEventHandler : IDomainEventHandler<CoursePreviewVideoUpdatedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public CoursePreviewVideoUpdatedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(CoursePreviewVideoUpdatedDomainEvent @event)
        {
            var newVideo = @event.Course.PreviewVideo;
            var oldVideo = @event.OldVideo;

            if (newVideo?.Status < VideoStatus.Processed && newVideo.FileId != oldVideo?.FileId)
            {
                _eventBus.Publish(new RequestVideoProcessingIntegrationEvent
                {
                    FileId = newVideo.FileId,
                    Url = newVideo.Url,
                    Entity = "course",
                    EntityId = @event.Course.Id,
                });
            }

            return Task.CompletedTask;
        }
    }
}