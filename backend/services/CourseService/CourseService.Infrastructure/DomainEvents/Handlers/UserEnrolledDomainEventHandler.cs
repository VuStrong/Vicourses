using CourseService.Domain.Events;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class UserEnrolledDomainEventHandler : IDomainEventHandler<UserEnrolledDomainEvent>
    {
        private readonly IMongoCollection<BsonDocument> _enrollmentCollection;
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly ILogger<UserEnrolledDomainEventHandler> _logger;

        public UserEnrolledDomainEventHandler(
            IMongoDatabase mongoDatabase,
            IMongoCollection<Course> courseCollection,
            ILogger<UserEnrolledDomainEventHandler> logger)
        {
            _enrollmentCollection = mongoDatabase.GetCollection<BsonDocument>("enrollments");
            _courseCollection = courseCollection;
            _logger = logger;
        }

        public async Task Handle(UserEnrolledDomainEvent @event)
        {
            var enrollment = new BsonDocument
            {
                { "_id", Guid.NewGuid().ToString() },
                { "CourseId", @event.Course.Id },
                { "UserId", @event.UserId },
                { "EnrolledAt", DateTime.Today },
            };

            await _enrollmentCollection.InsertOneAsync(enrollment);

            await _courseCollection.UpdateOneAsync(
                Builders<Course>.Filter.Eq(c => c.Id, @event.Course.Id),
                Builders<Course>.Update.Inc(c => c.StudentCount, 1)
            );

            _logger.LogInformation("[Course Service] User {msg1} enrolled course {msg2}", @event.UserId, @event.Course.Id);
        }
    }
}
