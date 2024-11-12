namespace DiscountService.API.Application.Dtos.Coupon
{
    public class CheckCouponDto
    {
        public required string Code { get; set; }
        public required string CourseId { get; set; }
        public required string UserId { get; set; }
    }

    public class CouponCheckResultDto
    {
        public string Code { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public int? Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? Remain { get; set; }
        public bool IsValid { get; set; }
        public string? Details { get; set; }
    }
}
