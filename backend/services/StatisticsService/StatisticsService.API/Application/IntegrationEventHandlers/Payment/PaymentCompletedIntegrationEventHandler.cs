using EventBus;
using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.IntegrationEvents.Payment;
using StatisticsService.API.Infrastructure;

namespace StatisticsService.API.Application.IntegrationEventHandlers.Payment
{
    public class PaymentCompletedIntegrationEventHandler : IIntegrationEventHandler<PaymentCompletedIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<PaymentCompletedIntegrationEventHandler> _logger;

        public PaymentCompletedIntegrationEventHandler(
            StatisticsServiceDbContext dbContext,
            ILogger<PaymentCompletedIntegrationEventHandler> logger
        )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(PaymentCompletedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle PaymentCompletedIntegrationEvent: {@event.Id}");

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.CourseId);

            if (course != null) 
            {
                var date = DateOnly.FromDateTime(@event.CreatedAt);
                var metric = await _dbContext.InstructorMetrics.FirstOrDefaultAsync(m =>
                    m.InstructorId == course.InstructorId &&
                    m.CourseId == course.Id &&
                    m.Date == date
                );

                if (metric != null)
                {
                    metric.IncreaseRevenue(@event.TotalPrice);
                }
                else
                {
                    metric = new Models.InstructorMetric(course.InstructorId, course.Id, date);
                    metric.IncreaseRevenue(@event.TotalPrice);
                    _dbContext.InstructorMetrics.Add(metric);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}