using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.IntegrationEvents.Course;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseInfoUpdatedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<CourseInfoUpdatedIntegrationEventHandler> _logger;

        public CourseInfoUpdatedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<CourseInfoUpdatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(CourseInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle CourseInfoUpdatedIntegrationEvent: {msg}", @event.Id);

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course == null)
            {
                course = new Models.Course(@event.Id, @event.Title, @event.User.Id, @event.Price);
                _dbContext.Courses.Add(course);
            }
            else
            {
                course.Title = @event.Title;
                course.Price = @event.Price;
                course.Status = CourseStatus.Published;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
