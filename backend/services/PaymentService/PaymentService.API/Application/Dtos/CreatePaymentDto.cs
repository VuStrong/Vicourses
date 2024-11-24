namespace PaymentService.API.Application.Dtos
{
    public class CreatePaymentDto
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string CourseId { get; set; }
        public required string CourseName { get; set; }
        public string? CouponCode { get; set; }
    }
}
