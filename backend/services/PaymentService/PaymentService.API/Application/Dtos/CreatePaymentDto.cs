namespace PaymentService.API.Application.Dtos
{
    public class CreatePaymentDto
    {
        public required string UserId { get; set; }
        public required string CourseId { get; set; }
        public string? CouponCode { get; set; }
    }
}
