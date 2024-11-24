using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Dtos.Paypal;
using PaymentService.API.Application.Exceptions;
using PaymentService.API.Application.IntegrationEvents;
using PaymentService.API.Application.Mappers;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;
using System.Threading;

namespace PaymentService.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly IDiscountService _discountService;
        private readonly IPaypalService _paypalService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            PaymentServiceDbContext dbContext,
            IDiscountService discountService,
            IPaypalService paypalService,
            IEventBus eventBus,
            ILogger<PaymentService> logger)
        {
            _dbContext = dbContext;
            _discountService = discountService;
            _paypalService = paypalService;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<PagedResult<PaymentDto>> GetUserPaymentsAsync(string userId, int skip, int limit, CancellationToken cancellationToken = default)
        {
            skip = skip < 0 ? 0 : skip;
            limit = limit <= 0 ? 15 : limit;

            var query = _dbContext.Payments.Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(limit);

            var total = await query.CountAsync(cancellationToken);

            var payments = await query.ToListAsync(cancellationToken);

            return new PagedResult<PaymentDto>(payments.ToPaymentDtos(), skip, limit, total);
        }

        public async Task<PaymentDto> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                throw new NotFoundException("payment", paymentId);
            }

            return payment.ToPaymentDto();
        }

        public async Task<PaymentDto> CreatePaypalPaymentAsync(CreatePaymentDto payload)
        {
            decimal listPrice, discount = 0, totalPrice;

            // If payload have couponCode, call discount service to check it, otherwise just get the course price.
            if (!string.IsNullOrEmpty(payload.CouponCode))
            {
                var checkResult = await _discountService.CheckCouponAsync(payload.CouponCode, payload.CourseId, payload.UserId);

                if (!checkResult.IsValid)
                {
                    throw new AppException(checkResult.Details!, 422);
                }

                listPrice = checkResult.Price;
                totalPrice = checkResult.DiscountPrice;
                discount = listPrice - totalPrice;
            }
            else
            {
                listPrice = await _discountService.GetCoursePriceAsync(payload.CourseId);
                totalPrice = listPrice;
            }

            var paypalOrder = await _paypalService.CreateOrderAsync(new CreatePaypalOrderDto
            {
                Intent = "CAPTURE",
                PurchaseUnits = [
                    new PaypalPurchaseUnitDto
                    {
                        Amount = new PaypalPurchaseUnitAmountDto
                        {
                            Value = totalPrice.ToString("F")
                        }
                    }
                ]
            });

            var payment = new Payment(payload.UserId, payload.Username, payload.Email, payload.CourseId, payload.CourseName,
                listPrice, discount, totalPrice, payload.CouponCode, PaymentMethod.Paypal, paypalOrder.Id);

            _dbContext.Payments.Add(payment);

            if (!string.IsNullOrEmpty(payment.CouponCode))
            {
                await _discountService.ConsumeCouponAsync(payment.CouponCode, payment.CourseId, payment.UserId);
            }

            await _dbContext.SaveChangesAsync();

            foreach (var link in paypalOrder.Links)
            {
                Console.WriteLine($"Rel:{link.Rel} Href:{link.Href}");
            }

            return payment.ToPaymentDto();
        }

        public async Task CapturePaypalPaymentAsync(string paypalOrderId, string userId)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.PaypalOrderId == paypalOrderId);

            if (payment == null)
            {
                throw new NotFoundException($"The paypal order '{paypalOrderId}' was not found");
            }
            if (payment.UserId != userId)
            {
                throw new AppException(
                    $"You do not have permission to capture this payment",
                    403);
            }
            if (payment.Status >= PaymentStatus.Completed)
            {
                throw new AppException(
                    $"The paypal payment '{paypalOrderId}' already completed",
                    422);
            }

            await _paypalService.CaptureOrderAsync(paypalOrderId);

            payment.Status = PaymentStatus.Completed;
            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(payment.ToPaymentCompletedIntegrationEvent());

            _eventBus.Publish(new SendEmailIntegrationEvent
            {
                To = payment.Email,
                Template = "payment_completed",
                Payload = payment
            });

            _logger.LogInformation($"Payment completed: {payment.Id}");
        }

        public async Task CancelPaymentAsync(string paymentId, string userId)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                throw new NotFoundException($"The payment '{paymentId}' was not found");
            }
            if (payment.UserId != userId)
            {
                throw new AppException(
                    $"You do not have permission to cancel this payment",
                    403);
            }
            if (payment.Status > PaymentStatus.Pending)
            {
                throw new AppException(
                    $"The payment '{paymentId}' already completed",
                    422);
            }

            payment.Status = PaymentStatus.Canceled;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Payment canceled: {paymentId}");
        }
    }
}
