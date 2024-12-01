using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.IntegrationEvents.Course;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;

        public CoursePublishedIntegrationEventHandler(
            PaymentServiceDbContext dbContext,
            ILogger<CoursePublishedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation("[Payment Service] Handle CoursePublishedIntegrationEvent: {msg}", @event.Id);

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
