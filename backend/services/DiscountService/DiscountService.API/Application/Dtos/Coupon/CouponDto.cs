namespace DiscountService.API.Application.Dtos.Coupon
{
    public class CouponDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Count { get; set; }
        public int Remain { get; set; }
        public int Discount { get; set; }
        public bool IsActive { get; set; }
    }
}
