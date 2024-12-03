using Quartz;
using PaymentService.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Models;
using PaymentService.API.Application.Services.Paypal;
using PaymentService.API.Application.Dtos.Paypal.Payouts;

namespace PaymentService.API.Application.BackgroundServices
{
    public class WeeklyPayoutsJob : IJob
    {
        private readonly ILogger<WeeklyPayoutsJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPaypalPayoutsService _paypalPayoutsService;

        public WeeklyPayoutsJob(
            ILogger<WeeklyPayoutsJob> logger, 
            IServiceProvider serviceProvider,
            IPaypalPayoutsService paypalPayoutsService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _paypalPayoutsService = paypalPayoutsService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("[Payment Service] Begin Payouts Job at {msg}", DateTime.Now.ToString());

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();

            // Since the refund policy is within 2 days.
            var end = DateTime.Now.Date.AddDays(-2);
            var start = end.AddDays(-7);
            var instructorPercent = 0.7m;

            var payments = await dbContext.Payments
                .Where(p => p.CreatedAt >= start &&
                    p.CreatedAt < end &&
                    p.Status == PaymentStatus.Completed)
                .GroupBy(p => p.CourseCreatorId)
                .Select(p => new
                {
                    CreatorId = p.Key,
                    Total = p.Sum(x => x.TotalPrice) * instructorPercent
                })
                .Join(
                    dbContext.Users, 
                    pg => pg.CreatorId,
                    u => u.Id,
                    (pg, u) => new
                    {
                        CreatorId = pg.CreatorId,
                        Total = pg.Total,
                        PaypalPayerId = u.PaypalPayerId,
                        PaypalEmail = u.PaypalEmail,
                    })
                .ToListAsync();

            if (payments.Count == 0) return;

            var payoutDto = new CreateBatchPayoutDto
            {
                SenderBatchHeader = new PayoutSenderBatchHeaderDto
                {
                    RecipientType = "PAYPAL_ID",
                    EmailSubject = "You have a payout from Vicourses",
                    EmailMessage = "You have received a payout for this week! Nice sell",
                }
            };

            foreach (var payment in payments)
            {
                if (string.IsNullOrEmpty(payment.PaypalPayerId))
                {
                    continue;
                }

                payoutDto.Items.Add(new PayoutItemDto
                {
                    Amount = new PayoutItemAmountDto
                    {
                        Value = payment.Total.ToString("F"),
                        Currency = "USD",
                    },
                    Receiver = payment.PaypalPayerId,
                });
            }

            if (payoutDto.Items.Count == 0) return;

            var senderBatchId = await _paypalPayoutsService.BatchPayoutAsync(payoutDto);

            var batchPayout = new BatchPayout("paypal", senderBatchId);
            dbContext.BatchPayouts.Add(batchPayout);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("[Payment Service] Payouts Job completed at {msg}", DateTime.Now.ToString());
        }
    }
}
