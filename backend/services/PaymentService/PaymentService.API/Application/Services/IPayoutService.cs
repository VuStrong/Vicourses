using PaymentService.API.Application.Dtos;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Services
{
    public interface IPayoutService
    {
        Task<PagedResult<BatchPayout>> GetPayoutsAsync(int skip, int limit, DateTime? from = null, DateTime? to = null,
            CancellationToken cancellationToken = default);

        Task<BatchPayout> GetPayoutAsync(string id, CancellationToken cancellationToken = default);
    }
}
