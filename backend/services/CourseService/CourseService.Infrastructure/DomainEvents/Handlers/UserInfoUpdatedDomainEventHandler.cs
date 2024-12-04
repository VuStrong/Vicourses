using CourseService.Domain.Events;
using CourseService.Domain.Events.User;
using CourseService.Domain.Models;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class UserInfoUpdatedDomainEventHandler : IDomainEventHandler<UserInfoUpdatedDomainEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Comment> _commentCollection;

        public UserInfoUpdatedDomainEventHandler(IMongoCollection<Course> courseCollection, IMongoCollection<Comment> commentCollection)
        {
            _courseCollection = courseCollection;
            _commentCollection = commentCollection;
        }

        public async Task Handle(UserInfoUpdatedDomainEvent @event)
        {
            if (@event.NameOrThumbnailUpdated)
            {
                var user = @event.User;
                var userInCourse = new UserInCourse(user.Id, user.Name, user.ThumbnailUrl);
                var userInComment = new UserInComment(user.Id, user.Name, user.ThumbnailUrl);

                await _courseCollection.UpdateManyAsync(
                    Builders<Course>.Filter.Eq(c => c.User.Id, user.Id),
                    Builders<Course>.Update.Set(c => c.User, userInCourse)
                );
                await _commentCollection.UpdateManyAsync(
                    Builders<Comment>.Filter.Eq(c => c.User.Id, user.Id),
                    Builders<Comment>.Update.Set(c => c.User, userInComment)
                );
            }
        }
    }
}
