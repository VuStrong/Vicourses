using EventBus;
using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.IntegrationEvents.Course;
using StatisticsService.API.Infrastructure;

namespace StatisticsService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;

        public CoursePublishedIntegrationEventHandler(
            StatisticsServiceDbContext dbContext,
            ILogger<CoursePublishedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle CoursePublishedIntegrationEvent: {@event.Id}");

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course == null)
            {
                course = new Models.Course(@event.Id, @event.User.Id);
                _dbContext.Courses.Add(course);
            }
            else
            {
                course.Status = Models.CourseStatus.Published;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
