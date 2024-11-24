using PaymentService.API.Application.Dtos;

namespace PaymentService.API.Application.Services
{
    public interface IDiscountService
    {
        Task<CouponCheckResultDto> CheckCouponAsync(string code, string courseId, string userId);
        Task ConsumeCouponAsync(string code, string courseId, string userId);
        Task<decimal> GetCoursePriceAsync(string courseId);
    }
}
