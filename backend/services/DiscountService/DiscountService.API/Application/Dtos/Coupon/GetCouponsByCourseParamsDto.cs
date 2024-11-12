namespace DiscountService.API.Application.Dtos.Coupon
{
    public class GetCouponsByCourseParamsDto
    {
        public required string CourseId { get; set; }
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public bool? IsExpired { get; set; }
    }
}
