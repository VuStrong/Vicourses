using DiscountService.API.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace DiscountService.API.Infrastructure.Cache
{
    public class CouponCachedRepository : ICouponCachedRepository
    {
        private readonly DiscountServiceDbContext _dbContext;
        private readonly IDatabase _redis;

        public CouponCachedRepository(DiscountServiceDbContext dbContext, IConnectionMultiplexer muxer)
        {
            _dbContext = dbContext;
            _redis = muxer.GetDatabase();
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            var key = $"coupon:{code}";
            var cachedCoupon = await _redis.StringGetAsync(key);

            Coupon? coupon = null;
            if (string.IsNullOrEmpty(cachedCoupon))
            {
                coupon = await _dbContext.Coupons.FirstOrDefaultAsync(
                    c => c.Code == code &&
                        c.ExpiryDate > DateTime.Today &&
                        c.Remain > 0 &&
                        c.IsActive);

                if (coupon != null)
                {
                    var value = JsonSerializer.Serialize(coupon);

                    await _redis.StringSetAsync(key, value, TimeSpan.FromHours(1));
                }

                return coupon;
            }

            coupon = JsonSerializer.Deserialize<Coupon>(cachedCoupon!);

            return coupon;
        }

        public async Task SetCouponAvailabilityAsync(Coupon coupon)
        {
            var key = $"coupon:{coupon.Code}:availability";
            var expiry = coupon.ExpiryDate - DateTime.UtcNow;

            await _redis.StringSetAsync(key, coupon.Remain.ToString(), expiry);
        }

        public async Task<int> DecreaseCouponAvailabilityAsync(string code)
        {
            var key = $"coupon:{code}:availability";

            var result = await _redis.StringDecrementAsync(key);

            return (int)result;
        }

        public async Task SetCouponUsedByUserAsync(Coupon coupon, string userId)
        {
            var key = $"coupon:{coupon.Code}:user:{userId}";
            var expiry = coupon.ExpiryDate - DateTime.UtcNow;

            await _redis.StringSetAsync(key, "true", expiry);
        }

        public async Task<bool> CheckCouponUsedByUserAsync(string code, string userId)
        {
            var key = $"coupon:{code}:user:{userId}";

            return !string.IsNullOrEmpty(await _redis.StringGetAsync(key));
        }

        public async Task DeleteCouponAsync(string code)
        {
            var key = $"coupon:{code}";
            await _redis.KeyDeleteAsync(key);
        }
    }
}
