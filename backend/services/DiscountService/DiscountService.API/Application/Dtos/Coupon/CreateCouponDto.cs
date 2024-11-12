namespace DiscountService.API.Application.Dtos.Coupon
{
    public class CreateCouponDto
    {
        public required string CourseId { get; set; }
        public required string UserId { get; set; }
        public required int Days { get; set; }
        public required int Availability { get; set; }
        public required int Discount { get; set; }
    }
}
