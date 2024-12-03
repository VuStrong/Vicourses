using CourseService.Domain.Events;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class SectionCreatedDomainEventHandler : IDomainEventHandler<SectionCreatedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;

        public SectionCreatedDomainEventHandler(IMongoCollection<Course> courseCollection)
        {
            _courseCollection = courseCollection;
        }

        public async Task Handle(SectionCreatedDomainEvent @event)
        {
            var filter = Builders<Course>.Filter.Eq(c => c.Id, @event.Section.CourseId);
            var update = Builders<Course>.Update.Inc(c => c.Metrics.SectionsCount, 1);

            await _courseCollection.UpdateOneAsync(filter, update);
        }
    }
}
