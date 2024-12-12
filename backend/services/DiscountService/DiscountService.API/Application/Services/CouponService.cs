using DiscountService.API.Application.Dtos;
using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Application.Exceptions;
using DiscountService.API.Extensions;
using DiscountService.API.Infrastructure;
using DiscountService.API.Infrastructure.Cache;
using DiscountService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.API.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly DiscountServiceDbContext _dbContext;
        private readonly ICouponCachedRepository _couponCachedRepository;
        private readonly ICourseCachedRepository _courseCachedRepository;
        private readonly ICouponCodeGenerator _couponCodeGenerator;

        public CouponService(
            DiscountServiceDbContext dbContext,
            ICouponCachedRepository couponCachedRepository,
            ICourseCachedRepository courseCachedRepository,
            ICouponCodeGenerator couponCodeGenerator)
        {
            _dbContext = dbContext;
            _couponCachedRepository = couponCachedRepository;
            _courseCachedRepository = courseCachedRepository;
            _couponCodeGenerator = couponCodeGenerator;
        }

        public async Task<PagedResult<CouponDto>> GetCouponsByCourseAsync(
            GetCouponsByCourseParamsDto paramsDto, CancellationToken cancellationToken = default)
        {
            int skip = paramsDto.Skip < 0 ? 0 : paramsDto.Skip;
            int limit = paramsDto.Limit <= 0 ? 10 : paramsDto.Limit;

            var query = _dbContext.Coupons.Where(c => c.CourseId == paramsDto.CourseId);

            if (paramsDto.IsExpired != null)
            {
                if (paramsDto.IsExpired.Value)
                {
                    query = query.Where(c => c.ExpiryDate <= DateTime.Today);
                }
                else
                {
                    query = query.Where(c => c.ExpiryDate > DateTime.Today);
                }
            }

            var total = await query.CountAsync(cancellationToken);

            query = query.OrderByDescending(c => c.CreatedAt).Skip(skip).Take(limit);
            var coupons = await query.ToListAsync(cancellationToken);

            return new PagedResult<CouponDto>(coupons.ToCouponsDto(), skip, limit, total);
        }

        public async Task<CouponDto> CreateCouponAsync(CreateCouponDto data)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == data.CourseId);
            if (course == null)
            {
                throw new NotFoundException("course", data.CourseId);
            }
            if (data.UserId != course.CreatorId)
            {
                throw new ForbiddenException("Only instructor of the course can create coupon");
            }
            if (course.IsFree)
            {
                throw new AppException(
                    "Cannot create coupon for the course because it free",
                    422);
            }

            var existsCoupon = await _dbContext.Coupons.AnyAsync(
                c => c.CourseId == course.Id && c.ExpiryDate > DateTime.Today);
            if (existsCoupon)
            {
                throw new AppException(
                    "This course course already have a coupon in use",
                    422);
            }

            var code = _couponCodeGenerator.Generate();
            var coupon = Coupon.Create(code, data.UserId, data.CourseId, data.Days, data.Availability, data.Discount);

            _dbContext.Coupons.Add(coupon);

            await _dbContext.SaveChangesAsync();

            await _couponCachedRepository.SetCouponAvailabilityAsync(coupon);

            return coupon.ToCouponDto();
        }

        public async Task SetCouponActiveAsync(SetCouponActiveDto data)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == data.Code);
            if (coupon == null)
            {
                throw new NotFoundException("coupon", data.Code);
            }

            if (data.UserId != coupon.CreatorId)
            {
                throw new ForbiddenException("Only creator of the coupon can update");
            }

            coupon.IsActive = data.IsActive;

            await _dbContext.SaveChangesAsync();

            await _couponCachedRepository.DeleteCouponAsync(coupon.Code);
        }

        public async Task<CouponCheckResultDto> CheckCouponAsync(CheckCouponDto data)
        {
            var result = new CouponCheckResultDto()
            {
                Code = data.Code,
                CourseId = data.CourseId,
            };

            var coupon = await _couponCachedRepository.GetCouponByCodeAsync(data.Code);

            if (coupon == null || coupon.CourseId != data.CourseId)
            {
                result.IsValid = false;
                result.Details = "Coupon is either invalid or does not apply to this course";

                return result;
            }

            if (await _couponCachedRepository.CheckCouponUsedByUserAsync(coupon.Code, data.UserId))
            {
                result.IsValid = false;
                result.Details = "You have already used this coupon";

                return result;
            }

            var coursePrice = await _courseCachedRepository.GetCoursePriceAsync(data.CourseId);

            if (coursePrice == null)
            {
                result.IsValid = false;
                result.Details = "The coupon entered is not valid";

                return result;
            }

            result.IsValid = true;
            result.Discount = coupon.Discount;
            result.Price = decimal.Round(coursePrice.Value, 2, MidpointRounding.AwayFromZero);
            result.Remain = coupon.Remain;

            var discountPrice = coursePrice.Value - coursePrice.Value * ((decimal)coupon.Discount / 100);
            result.DiscountPrice = decimal.Round(discountPrice, 2, MidpointRounding.AwayFromZero);

            return result;
        }

        public async Task ConsumeCouponAsync(CheckCouponDto data)
        {
            var coupon = await _couponCachedRepository.GetCouponByCodeAsync(data.Code);

            if (coupon == null || coupon.CourseId != data.CourseId)
            {
                throw new AppException(
                    $"Coupon '{data.Code}' is either invalid or does not apply to this course",
                    422);
            }
            if (await _couponCachedRepository.CheckCouponUsedByUserAsync(coupon.Code, data.UserId))
            {
                throw new AppException(
                    $"You have already used the coupon code {coupon.Code}",
                    422);
            }

            var remain = await _couponCachedRepository.DecreaseCouponAvailabilityAsync(data.Code);
            if (remain == 0)
            {
                await _couponCachedRepository.DeleteCouponAsync(coupon.Code);
            }
            else if (remain < 0)
            {
                throw new AppException(
                    $"Coupon '{data.Code}' is either invalid or does not apply to this course",
                    422);
            }

            await _couponCachedRepository.SetCouponUsedByUserAsync(coupon, data.UserId);

            await _dbContext.Coupons.Where(c => c.Code == coupon.Code)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.Remain, c => c.Remain - 1));
        }
    }
}
