using PaymentService.API.Models;

namespace PaymentService.API.Application.Dtos
{
    public class PaymentDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public DateTime RefundDueDate { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CouponCode { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod Method { get; set; } = PaymentMethod.Paypal;
        public string? PaypalOrderId { get; set; }
    }
}
