namespace DiscountService.API.Application.Services
{
    public interface ICouponCodeGenerator
    {
        string Generate(int length = 15);
    }
}
