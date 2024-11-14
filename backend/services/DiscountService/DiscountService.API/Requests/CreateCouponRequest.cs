using System.ComponentModel.DataAnnotations;

namespace DiscountService.API.Requests
{
    public class CreateCouponRequest
    {
        [Required]
        public string CourseId { get; set; } = null!;

        [Required]
        [Range(1, 15)]
        public int Days { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Availability { get; set; }

        [Required]
        [Range(5, 90)]
        public int Discount { get; set; }
    }
}
