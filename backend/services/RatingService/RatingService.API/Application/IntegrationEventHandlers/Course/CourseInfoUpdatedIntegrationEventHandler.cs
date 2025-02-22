using EventBus;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseInfoUpdatedIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<CourseInfoUpdatedIntegrationEventHandler> _logger;

        public CourseInfoUpdatedIntegrationEventHandler(
            RatingServiceDbContext context,
            ILogger<CourseInfoUpdatedIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(CourseInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle CourseInfoUpdatedIntegrationEvent: {@event.Id}");

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

        private async Task AddCourse(CourseInfoUpdatedIntegrationEvent @event)
        {
            var course = new Models.Course(@event.Id, @event.User.Id, @event.Title)
            {
                ThumbnailUrl = @event.ThumbnailUrl,
            };

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();
        }

        private async Task UpdateCourse(Models.Course course, CourseInfoUpdatedIntegrationEvent @event)
        {
            course.Title = @event.Title;
            course.ThumbnailUrl = @event.ThumbnailUrl;

            await _context.SaveChangesAsync();
        }
    }
}
