using System.ComponentModel.DataAnnotations;

namespace PaymentService.API.Requests
{
    public class CreatePaypalPaymentRequest
    {
        [Required]
        public string CourseId { get; set; } = null!;

        public string? CouponCode { get; set; }
    }
}
