using DiscountService.API.Application.IntegrationEvents.Course;
using DiscountService.API.Infrastructure;
using DiscountService.API.Infrastructure.Cache;
using EventBus;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.API.Application.IntegrationEventHandlers.Course
{
    public class CoursePublishedIntegrationEventHandler : IIntegrationEventHandler<CoursePublishedIntegrationEvent>
    {
        private readonly DiscountServiceDbContext _dbContext;
        private readonly ICourseCachedRepository _courseCachedRepository;
        private readonly ILogger<CoursePublishedIntegrationEventHandler> _logger;

        public CoursePublishedIntegrationEventHandler(
            DiscountServiceDbContext dbContext,
            ICourseCachedRepository courseCachedRepository,
            ILogger<CoursePublishedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _courseCachedRepository = courseCachedRepository;
            _logger = logger;
        }

        public async Task Handle(CoursePublishedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Discount Service] Handle CoursePublishedIntegrationEvent: {@event.Id}");

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.Id);

            if (course != null)
            {
                if (course.Price != @event.Price)
                {
                    course.Price = @event.Price;

                    await _dbContext.SaveChangesAsync();

                    await _courseCachedRepository.DeleteCoursePriceCachedAsync(course.Id);
                }
            }
            else
            {
                course = new Models.Course(@event.Id, @event.User.Id, @event.Price);
                _dbContext.Courses.Add(course);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
