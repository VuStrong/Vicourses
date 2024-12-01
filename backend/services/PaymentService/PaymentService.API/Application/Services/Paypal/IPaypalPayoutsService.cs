using PaymentService.API.Application.Dtos.Paypal.Payouts;

namespace PaymentService.API.Application.Services.Paypal
{
    public interface IPaypalPayoutsService
    {
        /// <summary>
        /// Creates a batch payout.
        /// </summary>
        /// <returns>The sender batch id</returns>
        Task<string> BatchPayoutAsync(CreateBatchPayoutDto payload);

        Task<object> GetBatchPayoutAsync(string id, int page = 0, int pageSize = 20);
    }
}
