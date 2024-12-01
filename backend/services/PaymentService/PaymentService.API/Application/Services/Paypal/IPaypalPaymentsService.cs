namespace PaymentService.API.Application.Services.Paypal
{
    public interface IPaypalPaymentsService
    {
        Task RefundCapturedPaymentAsync(string captureId);
    }
}
