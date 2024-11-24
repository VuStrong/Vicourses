using PaymentService.API.Application.Dtos;

namespace PaymentService.API.Application.Services
{
    public interface IPaymentService
    {
        Task<PagedResult<PaymentDto>> GetUserPaymentsAsync(string userId, int skip, int limit, CancellationToken cancellationToken = default);
        
        Task<PaymentDto> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default);

        Task<PaymentDto> CreatePaypalPaymentAsync(CreatePaymentDto payload);

        Task CapturePaypalPaymentAsync(string paypalOrderId, string userId);

        Task CancelPaymentAsync(string paymentId, string userId);
    }
}
