using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Dtos.Paypal.Orders;
using PaymentService.API.Application.Exceptions;
using PaymentService.API.Application.IntegrationEvents;
using PaymentService.API.Application.Mappers;
using PaymentService.API.Application.Services.Paypal;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentServiceDbContext _dbContext;
        private readonly IDiscountService _discountService;
        private readonly IPaypalOrdersService _paypalOrdersService;
        private readonly IPaypalPaymentsService _paypalPaymentsService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            PaymentServiceDbContext dbContext,
            IDiscountService discountService,
            IPaypalOrdersService paypalOrdersService,
            IPaypalPaymentsService paypalPaymentsService,
            IEventBus eventBus,
            ILogger<PaymentService> logger)
        {
            _dbContext = dbContext;
            _discountService = discountService;
            _paypalOrdersService = paypalOrdersService;
            _paypalPaymentsService = paypalPaymentsService;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<PagedResult<PaymentDto>> GetPaymentsAsync(GetPaymentsParamsDto paramsDto, CancellationToken cancellationToken = default)
        {
            int skip = paramsDto.Skip < 0 ? 0 : paramsDto.Skip;
            int limit = paramsDto.Limit <= 0 ? 15 : paramsDto.Limit;

            var query = _dbContext.Payments.AsQueryable();

            if (!string.IsNullOrEmpty(paramsDto.UserId))
            {
                query = query.Where(p => p.UserId == paramsDto.UserId);
            }
            if (paramsDto.Status != null)
            {
                query = query.Where(p => p.Status == paramsDto.Status.Value);
            }
            if (paramsDto.From != null)
            {
                query = query.Where(p => p.CreatedAt >= paramsDto.From.Value);
            }
            if (paramsDto.To != null)
            {
                query = query.Where(p => p.CreatedAt <= paramsDto.To.Value);
            }

            var total = await query.CountAsync(cancellationToken);
            var payments = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return new PagedResult<PaymentDto>(payments.ToPaymentDtos(), skip, limit, total);
        }

        public async Task<PagedResult<PaymentDto>> GetUserPaymentsAsync(string userId, int skip, int limit, PaymentStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            skip = skip < 0 ? 0 : skip;
            limit = limit <= 0 ? 15 : limit;

            var query = _dbContext.Payments.Where(p => p.UserId == userId);
                
            if (status != null)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            var total = await query.CountAsync(cancellationToken);

            var payments = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return new PagedResult<PaymentDto>(payments.ToPaymentDtos(), skip, limit, total);
        }

        public async Task<PaymentDto> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);

            if (payment == null)
            {
                throw new NotFoundException("payment", paymentId);
            }

            return payment.ToPaymentDto();
        }

        public async Task<PaymentDto> CreatePaypalPaymentAsync(CreatePaymentDto payload)
        {
            if (await _dbContext.Payments.AnyAsync(p => 
                p.UserId == payload.UserId &&
                p.CourseId == payload.CourseId &&
                p.Status == PaymentStatus.Completed))
            {
                throw new AppException("You already purchased this course", 422);
            }

            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == payload.CourseId);
            if (course == null || course.Status != CourseStatus.Published)
            {
                throw new NotFoundException($"The course '{payload.CourseId}' was not found");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == payload.UserId);
            if (user == null)
            {
                throw new NotFoundException($"The user '{payload.UserId}' was not found");
            }

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
                listPrice = course.Price;
                totalPrice = listPrice;
            }

            if (totalPrice == 0)
            {
                throw new AppException("Cannot create payment for a free course", 422);
            }

            var paypalOrder = await _paypalOrdersService.CreateOrderAsync(new CreatePaypalOrderDto
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

            var payment = new Payment(user.Id, user.Name, user.Email, course.Id, course.Title, course.CreatorId,
                listPrice, discount, totalPrice, payload.CouponCode, PaymentMethod.Paypal, paypalOrder.Id);

            _dbContext.Payments.Add(payment);

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

            if (!string.IsNullOrEmpty(payment.CouponCode))
            {
                await _discountService.ConsumeCouponAsync(payment.CouponCode, payment.CourseId, payment.UserId);
            }

            await _paypalOrdersService.CaptureOrderAsync(paypalOrderId);

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

            _dbContext.Payments.Remove(payment);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Payment canceled: {paymentId}");
        }

        public async Task RefundPaymentAsync(string paymentId, string userId, string? reason = null)
        {
            var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
            {
                throw new NotFoundException($"The payment '{paymentId}' was not found");
            }
            if (payment.UserId != userId)
            {
                throw new AppException(
                    $"You do not have permission to refund this payment",
                    403);
            }
            if (payment.Status != PaymentStatus.Completed)
            {
                throw new AppException(
                    $"The payment '{paymentId}' must be completed to make a refund",
                    422);
            }

            if (payment.RefundDueDate <= DateTime.Now)
            {
                throw new AppException(
                    $"The payment '{paymentId}' can only be refunded within 2 days",
                    422);
            }

            if (payment.Method == PaymentMethod.Paypal)
            {
                var paypalOrder = await _paypalOrdersService.GetOrderAsync(payment.PaypalOrderId!);

                await _paypalPaymentsService.RefundCapturedPaymentAsync(paypalOrder.PurchaseUnits[0].Payments!.Captures[0].Id);
            }
            // Any future payment methods
            else
            {
                throw new AppException("Error", 500);
            }

            payment.Status = PaymentStatus.Refunded;
            payment.RefundReason = reason;

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(payment.ToPaymentRefundedIntegrationEvent());

            _eventBus.Publish(new SendEmailIntegrationEvent
            {
                To = payment.Email,
                Template = "payment_refunded",
                Payload = payment
            });

            _logger.LogInformation($"Payment refunded: {payment.Id}");
        }
    }
}
