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
            var date = DateOnly.FromDateTime(DateTime.Now);

            if (course != null) 
            {
                var metric = await _dbContext.InstructorMetrics.FirstOrDefaultAsync(m =>
                    m.InstructorId == course.InstructorId &&
                    m.CourseId == course.Id &&
                    m.Date == date
                );
                var totalPrice = @event.TotalPrice * 0.7m;

                if (metric != null)
                {
                    metric.IncreaseRevenue(totalPrice);
                }
                else
                {
                    metric = new Models.InstructorMetric(course.InstructorId, course.Id, date);
                    metric.IncreaseRevenue(totalPrice);
                    _dbContext.InstructorMetrics.Add(metric);
                }

            }

            var adminTotalPrice = @event.TotalPrice * 0.3m;

            var adminMetric = await _dbContext.AdminMetrics.FirstOrDefaultAsync(m => m.Date == date);
            if (adminMetric == null)
            {
                adminMetric = new Models.AdminMetric(date);
                adminMetric.Revenue += adminTotalPrice;
                _dbContext.AdminMetrics.Add(adminMetric);
            }
            else
            {
                adminMetric.Revenue += adminTotalPrice;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}