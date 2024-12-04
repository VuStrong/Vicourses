using CourseService.Domain.Enums;
using CourseService.Domain.Events;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class LessonDeletedDomainEventHandler : IDomainEventHandler<LessonDeletedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Lesson> _lessonCollection;

        public LessonDeletedDomainEventHandler(IMongoCollection<Course> courseCollection, IMongoCollection<Lesson> lessonCollection)
        {
            _courseCollection = courseCollection;
            _lessonCollection = lessonCollection;
        }

        public async Task Handle(LessonDeletedDomainEvent @event)
        {
            var lesson = @event.Lesson;
            var lessonFilter = Builders<Lesson>.Filter.Eq(l => l.Id, lesson.Id);
            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, lesson.CourseId);
            var courseUpdate = Builders<Course>.Update.Inc(c => c.Metrics.LessonsCount, -1);

            if (lesson.Type == LessonType.Quiz)
            {
                courseUpdate = courseUpdate.Inc(c => c.Metrics.QuizLessonsCount, -1);
            }
            else if (lesson.Video != null)
            {
                courseUpdate = courseUpdate.Inc(c => c.Metrics.TotalVideoDuration, -lesson.Video.Duration);
            }

            await _lessonCollection.DeleteOneAsync(lessonFilter);
            await _courseCollection.UpdateOneAsync(courseFilter, courseUpdate);
        }
    }
}
