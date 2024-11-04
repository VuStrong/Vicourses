using EventBus;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;

        public CoursePublishedIntegrationEventHandler(
            RatingServiceDbContext context,
            ILogger<CoursePublishedIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle CoursePublishedIntegrationEvent: {@event.Id}");

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course != null)
            {
                await UpdateCourse(course, @event);
            }
            else
            {
                await AddCourse(@event);
            }
        }

        private async Task AddCourse(CoursePublishedIntegrationEvent @event)
        {
            var course = new Models.Course(@event.Id, @event.User.Id);

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();
        }

        private async Task UpdateCourse(Models.Course course, CoursePublishedIntegrationEvent @event)
        {
            course.Status = Models.CourseStatus.Published;

            await _context.SaveChangesAsync();
        }
    }
}
