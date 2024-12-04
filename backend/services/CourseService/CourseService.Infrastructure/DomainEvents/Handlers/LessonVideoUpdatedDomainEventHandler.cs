using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class LessonVideoUpdatedDomainEventHandler : IDomainEventHandler<LessonVideoUpdatedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public LessonVideoUpdatedDomainEventHandler(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task Handle(LessonVideoUpdatedDomainEvent @event)
        {
            var newVideoDuration = @event.Lesson.Video?.Duration ?? 0;
            var oldVideoDuration = @event.OldVideo?.Duration ?? 0;

            var filter = Builders<Course>.Filter.Eq(c => c.Id, @event.Lesson.CourseId);
            var update = Builders<Course>.Update.Inc(
                c => c.Metrics.TotalVideoDuration,
                newVideoDuration - oldVideoDuration
            );

            await _courseCollection.UpdateOneAsync(filter, update);
        }
    }
}
