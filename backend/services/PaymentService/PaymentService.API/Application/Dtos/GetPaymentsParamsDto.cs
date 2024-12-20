using PaymentService.API.Models;

namespace PaymentService.API.Application.Dtos
{
    public class GetPaymentsParamsDto
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
        public string? UserId { get; set; }
        public PaymentStatus? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}