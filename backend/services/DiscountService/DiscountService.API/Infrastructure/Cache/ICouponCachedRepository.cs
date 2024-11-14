using DiscountService.API.Models;

namespace DiscountService.API.Infrastructure.Cache
{
    public interface ICouponCachedRepository
    {
        Task<Coupon?> GetCouponByCodeAsync(string code);

        Task SetCouponAvailabilityAsync(Coupon coupon);

        /// <summary>
        /// Decrease the coupon availability by its code
        /// </summary>
        /// <returns>The availability after decrease</returns>
        Task<int> DecreaseCouponAvailabilityAsync(string code);

        Task SetCouponUsedByUserAsync(Coupon coupon, string userId);
        Task<bool> CheckCouponUsedByUserAsync(string code, string userId);

        Task DeleteCouponAsync(string code);
    }
}
