using CourseService.Domain.Events;
using CourseService.Domain.Events.Section;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class SectionDeletedDomainEventHandler : IDomainEventHandler<SectionDeletedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Section> _sectionCollection;

        public SectionDeletedDomainEventHandler(IMongoCollection<Course> courseCollection, IMongoCollection<Section> sectionCollection)
        {
            _courseCollection = courseCollection;
            _sectionCollection = sectionCollection;
        }

        public async Task Handle(SectionDeletedDomainEvent @event)
        {
            var sectionFilter = Builders<Section>.Filter.Eq(s => s.Id, @event.Section.Id);
            var courseFilter = Builders<Course>.Filter.Eq(c => c.Id, @event.Section.CourseId);
            var courseUpdate = Builders<Course>.Update.Inc(c => c.Metrics.SectionsCount, -1);

            await _sectionCollection.DeleteOneAsync(sectionFilter);
            await _courseCollection.UpdateOneAsync(courseFilter, courseUpdate);
        }
    }
}
