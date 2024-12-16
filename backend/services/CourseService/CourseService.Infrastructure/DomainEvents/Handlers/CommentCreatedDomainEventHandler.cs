using CourseService.Domain.Events;
using CourseService.Domain.Events.Comment;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    public class CommentCreatedDomainEventHandler : IDomainEventHandler<CommentCreatedDomainEvent>
    {
        private readonly IMongoCollection<Comment> _commentCollection;

        public CommentCreatedDomainEventHandler(IMongoCollection<Comment> commentCollection)
        {
            _commentCollection = commentCollection;
        }

        public async Task Handle(CommentCreatedDomainEvent @event)
        {
            if (@event.Comment.ReplyToId != null)
            {
                var filter = Builders<Comment>.Filter.Eq(c => c.Id, @event.Comment.ReplyToId);
                var update = Builders<Comment>.Update.Inc(c => c.ReplyCount, 1);

                await _commentCollection.UpdateOneAsync(filter, update);
            }
        }
    }
}