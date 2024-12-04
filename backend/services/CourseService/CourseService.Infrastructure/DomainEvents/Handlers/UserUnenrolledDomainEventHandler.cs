using CourseService.Domain.Events;
using CourseService.Domain.Events.Enrollment;
using CourseService.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CourseService.Infrastructure.DomainEvents.Handlers
{
    internal class UserUnenrolledDomainEventHandler : IDomainEventHandler<UserUnenrolledDomainEvent>
    {
        private readonly IMongoCollection<Enrollment> _enrollmentCollection;
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly ILogger<UserUnenrolledDomainEventHandler> _logger;

        public UserUnenrolledDomainEventHandler(
            IMongoCollection<Enrollment> enrollmentCollection,
            IMongoCollection<Course> courseCollection,
            ILogger<UserUnenrolledDomainEventHandler> logger)
        {
            _enrollmentCollection = enrollmentCollection;
            _courseCollection = courseCollection;
            _logger = logger;
        }

        public async Task Handle(UserUnenrolledDomainEvent @event)
        {
            if (await CheckEnrollmentAsync(@event.Course.Id, @event.UserId))
            {
                await _enrollmentCollection.DeleteOneAsync(e => e.CourseId == @event.Course.Id && e.UserId == @event.UserId);

                await _courseCollection.UpdateOneAsync(
                    Builders<Course>.Filter.Eq(c => c.Id, @event.Course.Id),
                    Builders<Course>.Update.Inc(c => c.StudentCount, -1)
                );

                _logger.LogInformation("[Course Service] User {msg1} unenrolled course {msg2}", @event.UserId, @event.Course.Id);
            }
        }

        private async Task<bool> CheckEnrollmentAsync(string courseId, string userId)
        {
            return await _enrollmentCollection.Find(e => e.CourseId == courseId && e.UserId == userId).AnyAsync();
        }
    }
}
