using System.ComponentModel.DataAnnotations;

namespace PaymentService.API.Requests
{
    public class CreatePaypalPaymentRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string CourseId { get; set; } = null!;

        [Required]
        public string CourseName { get; set; } = null!;

        public string? CouponCode { get; set; }
    }
}
