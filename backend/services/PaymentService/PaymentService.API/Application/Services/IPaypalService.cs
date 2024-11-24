using PaymentService.API.Application.Dtos.Paypal;

namespace PaymentService.API.Application.Services
{
    public interface IPaypalService
    {
        Task<string> GetAccessTokenAsync();

        Task<PaypalOrderDto> GetOrderAsync(string orderId);

        Task<PaypalOrderDto> CreateOrderAsync(CreatePaypalOrderDto payload);

        Task<PaypalOrderDto> CaptureOrderAsync(string orderId);
    }
}
