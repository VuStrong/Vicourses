using EventBus;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Infrastructure;

namespace RatingService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseUnpublishedIntegrationEventHandler : IIntegrationEventHandler<CourseUnpublishedIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<CourseUnpublishedIntegrationEventHandler> _logger;

        public CourseUnpublishedIntegrationEventHandler(
            RatingServiceDbContext context,
            ILogger<CourseUnpublishedIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(CourseUnpublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle CourseUnpublishedIntegrationEvent: {@event.Id}");

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course != null)
            {
                course.Status = Models.CourseStatus.Unpublished;

                await _context.SaveChangesAsync();
            }
        }
    }
}
