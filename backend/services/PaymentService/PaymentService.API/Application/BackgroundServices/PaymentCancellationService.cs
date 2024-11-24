using Microsoft.EntityFrameworkCore;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.BackgroundServices
{
    public class PaymentCancellationService : BackgroundService
    {
        private readonly ILogger<PaymentCancellationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PaymentCancellationService(ILogger<PaymentCancellationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                await CheckAndCancelPaymentsAsync(stoppingToken);
            }
        }

        private async Task CheckAndCancelPaymentsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();
                
                await dbContext.Payments
                    .Where(p => p.Status == PaymentStatus.Pending && p.PaymentDueDate < DateTime.Now)
                    .ExecuteDeleteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when running cancel payments background service: {ex.Message}");
            }
        }
    }
}