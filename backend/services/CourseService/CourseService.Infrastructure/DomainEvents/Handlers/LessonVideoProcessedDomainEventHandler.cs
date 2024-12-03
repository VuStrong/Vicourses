using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class LessonVideoProcessedDomainEventHandler : IDomainEventHandler<LessonVideoProcessedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public LessonVideoProcessedDomainEventHandler(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task Handle(LessonVideoProcessedDomainEvent @event)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, @event.Lesson.CourseId);
            var update = Builders<Course>.Update.Inc(c => c.Metrics.TotalVideoDuration, @event.Lesson.Video?.Duration ?? 0);

            await _courseCollection.UpdateOneAsync(filter, update);
        }
    }
}
