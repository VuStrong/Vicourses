namespace DiscountService.API.Application.Dtos.Coupon
{
    public class SetCouponActiveDto
    {
        public string Code { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }

        public SetCouponActiveDto(string code, string userId, bool isActive)
        {
            Code = code;
            UserId = userId;
            IsActive = isActive;
        }
    }
}
