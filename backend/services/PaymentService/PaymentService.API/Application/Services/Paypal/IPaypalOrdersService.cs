using PaymentService.API.Application.Dtos.Paypal.Orders;

namespace PaymentService.API.Application.Services.Paypal
{
    public interface IPaypalOrdersService
    {
        Task<PaypalOrderDto> GetOrderAsync(string orderId);

        Task<PaypalOrderDto> CreateOrderAsync(CreatePaypalOrderDto payload);

        Task<PaypalOrderDto> CaptureOrderAsync(string orderId);
    }
}
