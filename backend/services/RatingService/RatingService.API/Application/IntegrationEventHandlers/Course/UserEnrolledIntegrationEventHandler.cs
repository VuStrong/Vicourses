using EventBus;
using RatingService.API.Application.IntegrationEvents.Course;
using RatingService.API.Infrastructure;
using RatingService.API.Models;

namespace RatingService.API.Application.IntegrationEventHandlers.Course
{
    public class UserEnrolledIntegrationEventHandler : IIntegrationEventHandler<UserEnrolledIntegrationEvent>
    {
        private readonly RatingServiceDbContext _context;
        private readonly ILogger<UserEnrolledIntegrationEventHandler> _logger;

        public UserEnrolledIntegrationEventHandler(
            RatingServiceDbContext context,
            ILogger<UserEnrolledIntegrationEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(UserEnrolledIntegrationEvent @event)
        {
            _logger.LogInformation($"[Rating Service] Handle UserEnrolledIntegrationEvent: {@event.UserId} {@event.Course.Id}");

            var enrollment = new Enrollment(@event.Course.Id, @event.UserId);

            _context.Enrollments.Add(enrollment);

            await _context.SaveChangesAsync();
        }
    }
}
