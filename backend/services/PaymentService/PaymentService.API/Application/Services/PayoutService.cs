using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Exceptions;
using PaymentService.API.Infrastructure;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Services
{
    public class PayoutService : IPayoutService
    {
        private readonly PaymentServiceDbContext _dbContext;

        public PayoutService(PaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<BatchPayout>> GetPayoutsAsync(int skip, int limit, DateTime? from = null, DateTime? to = null, 
            CancellationToken cancellationToken = default)
        {
            skip = skip < 0 ? 0 : skip;
            limit = limit <= 0 ? 15 : limit;

            var query = _dbContext.BatchPayouts.AsQueryable();

            if (from != null)
            {
                query = query.Where(bp => bp.Date >= from.Value);
            }
            if (to != null)
            {
                query = query.Where(bp => bp.Date < to.Value);
            }

            var total = await query.CountAsync(cancellationToken);

            var payouts = await query
                .OrderByDescending(bp => bp.Date)
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            return new PagedResult<BatchPayout>(payouts, skip, limit, total);
        }

        public async Task<BatchPayout> GetPayoutAsync(string id, CancellationToken cancellationToken = default)
        {
            var payout = await _dbContext.BatchPayouts.FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);

            if (payout == null)
            {
                throw new NotFoundException($"Batch payout '{id}' was not found");
            }

            return payout;
        }
    }
}
