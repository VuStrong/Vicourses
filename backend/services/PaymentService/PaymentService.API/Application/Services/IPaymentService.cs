using PaymentService.API.Application.Dtos;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Services
{
    public interface IPaymentService
    {
        Task<PagedResult<PaymentDto>> GetPaymentsAsync(GetPaymentsParamsDto paramsDto, CancellationToken cancellationToken = default);
        Task<PagedResult<PaymentDto>> GetUserPaymentsAsync(string userId, int skip, int limit, PaymentStatus? status = null,
            CancellationToken cancellationToken = default);
        
        Task<PaymentDto> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default);

        Task<PaymentDto> CreatePaypalPaymentAsync(CreatePaymentDto payload);

        Task CapturePaypalPaymentAsync(string paypalOrderId, string userId);

        Task CancelPaymentAsync(string paymentId, string userId);

        Task RefundPaymentAsync(string paymentId, string userId, string? reason = null);
    }
}
