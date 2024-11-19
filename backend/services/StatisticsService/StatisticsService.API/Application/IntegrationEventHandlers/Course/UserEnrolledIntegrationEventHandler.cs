using EventBus;
using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.IntegrationEvents.Course;
using StatisticsService.API.Infrastructure;

namespace StatisticsService.API.Application.IntegrationEventHandlers.Course
{
    public class UserEnrolledIntegrationEventHandler : IIntegrationEventHandler<UserEnrolledIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<UserEnrolledIntegrationEventHandler> _logger;

        public UserEnrolledIntegrationEventHandler(
            StatisticsServiceDbContext dbContext, 
            ILogger<UserEnrolledIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(UserEnrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle UserEnrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var metric = await _dbContext.InstructorMetrics.FirstOrDefaultAsync(m =>
                m.InstructorId == @event.Course.User.Id &&
                m.CourseId == @event.Course.Id &&
                m.Date == @event.EnrolledAt
            );

            if (metric != null)
            {
                metric.IncreaseEnrollmentCount();
            }
            else
            {
                metric = new Models.InstructorMetric(@event.Course.User.Id, @event.Course.Id, @event.EnrolledAt);
                metric.IncreaseEnrollmentCount();
                _dbContext.InstructorMetrics.Add(metric);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
