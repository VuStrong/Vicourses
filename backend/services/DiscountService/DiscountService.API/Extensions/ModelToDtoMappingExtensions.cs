using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Models;

namespace DiscountService.API.Extensions
{
    public static class ModelToDtoMappingExtensions
    {
        public static CouponDto ToCouponDto(this Coupon coupon)
        {
            return new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                CreatorId = coupon.CreatorId,
                CourseId = coupon.CourseId,
                CreatedAt = coupon.CreatedAt,
                ExpiryDate = coupon.ExpiryDate,
                Count = coupon.Count,
                Remain = coupon.Remain,
                Discount = coupon.Discount,
                IsActive = coupon.IsActive,
            };
        }

        public static IEnumerable<CouponDto> ToCouponsDto(this IEnumerable<Coupon> coupons)
        {
            return coupons.Select(coupon => new CouponDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                CreatorId = coupon.CreatorId,
                CourseId = coupon.CourseId,
                CreatedAt = coupon.CreatedAt,
                ExpiryDate = coupon.ExpiryDate,
                Count = coupon.Count,
                Remain = coupon.Remain,
                Discount = coupon.Discount,
                IsActive = coupon.IsActive,
            });
        }
    }
}
