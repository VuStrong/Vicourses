using System.Security.Cryptography;
using System.Text;

namespace DiscountService.API.Application.Services
{
    public class CouponCodeGenerator : ICouponCodeGenerator
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string Generate(int length = 15)
        {
            if (length < 15)
                throw new ArgumentException("Coupon code must be at least 15 characters long.");
    
            var couponCode = new StringBuilder(length);
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[length];
                rng.GetBytes(bytes);

                foreach (var byteValue in bytes)
                {
                    var randomIndex = byteValue % Characters.Length;
                    couponCode.Append(Characters[randomIndex]);
                }
            }

            return couponCode.ToString();
        }
    }
}
