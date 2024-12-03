using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class LessonCreatedDomainEventHandler : IDomainEventHandler<LessonCreatedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public LessonCreatedDomainEventHandler(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task Handle(LessonCreatedDomainEvent @event)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, @event.Lesson.CourseId);
            var update = Builders<Course>.Update.Inc(c => c.Metrics.LessonsCount, 1);

            if (@event.Lesson.Type == LessonType.Quiz)
            {
                update = update.Inc(c => c.Metrics.QuizLessonsCount, 1);
            }

            await _courseCollection.UpdateOneAsync(filter, update);
        }
    }
}
