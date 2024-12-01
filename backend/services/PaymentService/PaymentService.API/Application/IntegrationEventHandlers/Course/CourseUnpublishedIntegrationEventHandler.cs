using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.IntegrationEvents.Course;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseUnpublishedIntegrationEventHandler : IIntegrationEventHandler<CourseUnpublishedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<CourseUnpublishedIntegrationEventHandler> _logger;

        public CourseUnpublishedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<CourseUnpublishedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(CourseUnpublishedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle CourseUnpublishedIntegrationEvent: {msg}", @event.Id);

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course != null)
            {
                course.Status = CourseStatus.Unpublished;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
