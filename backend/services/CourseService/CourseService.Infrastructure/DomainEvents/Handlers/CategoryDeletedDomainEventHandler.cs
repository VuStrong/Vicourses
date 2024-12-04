using CourseService.Domain.Events;
using CourseService.Domain.Events.Category;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class CategoryDeletedDomainEventHandler : IDomainEventHandler<CategoryDeletedDomainEvent>
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryDeletedDomainEventHandler(IMongoCollection<Category> categoryCollection)
        {
            _categoryCollection = categoryCollection;
        }

        public async Task Handle(CategoryDeletedDomainEvent @event)
        {
            var filter = Builders<Category>.Filter.Eq("_id", @event.Category.Id);

            await _categoryCollection.DeleteOneAsync(filter);
        }
    }
}
