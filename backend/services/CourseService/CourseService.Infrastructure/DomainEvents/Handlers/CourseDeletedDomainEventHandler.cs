using CourseService.Domain.Events;
using CourseService.Domain.Events.Course;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class CourseDeletedDomainEventHandler : IDomainEventHandler<CourseDeletedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public CourseDeletedDomainEventHandler(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task Handle(CourseDeletedDomainEvent @event)
        {
            var filter = Builders<Course>.Filter.Eq("_id", @event.Course.Id);

            await _courseCollection.DeleteOneAsync(filter);
        }
    }
}
