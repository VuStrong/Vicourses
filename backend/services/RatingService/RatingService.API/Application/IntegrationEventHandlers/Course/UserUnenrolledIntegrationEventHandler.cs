using EventBus;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.Course
{
    public class UserUnenrolledIntegrationEventHandler : IIntegrationEventHandler<UserUnenrolledIntegrationEvent>
    {
        private readonly RatingServiceDbContext _dbContext;
        private readonly ILogger<UserUnenrolledIntegrationEventHandler> _logger;

        public UserUnenrolledIntegrationEventHandler(
            RatingServiceDbContext dbContext,
            ILogger<UserUnenrolledIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserUnenrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle UserUnenrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var enrollment = await _dbContext.Enrollments.FirstOrDefaultAsync(e =>
                e.CourseId == @event.Course.Id && e.UserId == @event.UserId);

            if (enrollment != null)
            {
                _dbContext.Enrollments.Remove(enrollment);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
