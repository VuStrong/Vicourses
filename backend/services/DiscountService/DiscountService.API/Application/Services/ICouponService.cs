using DiscountService.API.Application.Dtos;
using DiscountService.API.Application.Dtos.Coupon;

namespace DiscountService.API.Application.Services
{
    public interface ICouponService
    {
        Task<PagedResult<CouponDto>> GetCouponsByCourseAsync(
            GetCouponsByCourseParamsDto paramsDto, CancellationToken cancellationToken = default);

        Task<CouponDto> CreateCouponAsync(CreateCouponDto data);

        Task SetCouponActiveAsync(SetCouponActiveDto data);

        Task<CouponCheckResultDto> CheckCouponAsync(CheckCouponDto data);
        Task ConsumeCouponAsync(CheckCouponDto data);
    }
}
