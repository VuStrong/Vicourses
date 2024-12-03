using EventBus;
using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.IntegrationEvents.Payment;
using StatisticsService.API.Infrastructure;

namespace StatisticsService.API.Application.IntegrationEventHandlers.Payment
{
    public class PaymentRefundedIntegrationEventHandler : IIntegrationEventHandler<PaymentRefundedIntegrationEvent>
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly ILogger<PaymentRefundedIntegrationEventHandler> _logger;

        public PaymentRefundedIntegrationEventHandler(
            StatisticsServiceDbContext dbContext,
            ILogger<PaymentRefundedIntegrationEventHandler> logger
        )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(PaymentRefundedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Statistics Service] Handle PaymentRefundedIntegrationEvent: {@event.Id}");

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == @event.CourseId);
            var date = DateOnly.FromDateTime(DateTime.Now);

            if (course != null)
            {
                var metric = await _dbContext.InstructorMetrics.FirstOrDefaultAsync(m =>
                    m.InstructorId == course.InstructorId &&
                    m.CourseId == course.Id &&
                    m.Date == date
                );

                if (metric != null)
                {
                    metric.IncreaseRefundCount();
                }
                else
                {
                    metric = new Models.InstructorMetric(course.InstructorId, course.Id, date);
                    metric.IncreaseRefundCount();
                    _dbContext.InstructorMetrics.Add(metric);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
