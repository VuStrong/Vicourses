using DiscountService.API.Application.IntegrationEvents.Course;
using DiscountService.API.Infrastructure;
using DiscountService.API.Infrastructure.Cache;
using EventBus;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.API.Application.IntegrationEventHandlers.Course
{
    public class CourseInfoUpdatedIntegrationEventHandler : IIntegrationEventHandler<CourseInfoUpdatedIntegrationEvent>
    {
        private readonly DiscountServiceDbContext _dbContext;
        private readonly ICourseCachedRepository _courseCachedRepository;
        private readonly ILogger<CourseInfoUpdatedIntegrationEventHandler> _logger;

        public CourseInfoUpdatedIntegrationEventHandler(
            DiscountServiceDbContext dbContext, 
            ICourseCachedRepository courseCachedRepository,
            ILogger<CourseInfoUpdatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext;
            _courseCachedRepository = courseCachedRepository;
            _logger = logger;
        }

        public async Task Handle(CourseInfoUpdatedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Discount Service] Handle CourseInfoUpdatedIntegrationEvent: {@event.Id}");

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
