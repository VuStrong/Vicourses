using CourseService.Application.Exceptions;
using CourseService.Application.IntegrationEvents.Rating;
using CourseService.Domain.Contracts;
using EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.Rating
{
    public class CourseRatingUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseRatingUpdatedIntegrationEvent>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseRatingUpdatedIntegrationEventHandler> _logger;

        public CourseRatingUpdatedIntegrationEventHandler(
            ICourseRepository courseRepository,
            ILogger<CourseRatingUpdatedIntegrationEventHandler> logger
        )
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task Handle(CourseRatingUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Course Service] Handle CourseRatingUpdatedIntegrationEvent: {@event.Id}");

            var course = await _courseRepository.FindOneAsync(@event.Id) ?? throw new CourseNotFoundException(@event.Id);

            course.SetRating(@event.AvgRating);

            await _courseRepository.UpdateAsync(course);
        }
    }
}