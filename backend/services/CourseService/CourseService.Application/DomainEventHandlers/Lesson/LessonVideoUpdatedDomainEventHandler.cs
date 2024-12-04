using CourseService.Application.IntegrationEvents.VideoProcessing;
using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using EventBus;

namespace CourseService.Application.DomainEventHandlers.Lesson
{
    internal class LessonVideoUpdatedDomainEventHandler : IDomainEventHandler<LessonVideoUpdatedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public LessonVideoUpdatedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(LessonVideoUpdatedDomainEvent @event)
        {
            var newVideo = @event.Lesson.Video;
            var oldVideo = @event.OldVideo;

            if (newVideo?.Status < VideoStatus.Processed && newVideo.FileId != oldVideo?.FileId)
            {
                _eventBus.Publish(new RequestVideoProcessingIntegrationEvent
                {
                    FileId = newVideo.FileId,
                    Url = newVideo.Url,
                    Entity = "lesson",
                    EntityId = @event.Lesson.Id,
                });
            }

            return Task.CompletedTask;
        }
    }
}