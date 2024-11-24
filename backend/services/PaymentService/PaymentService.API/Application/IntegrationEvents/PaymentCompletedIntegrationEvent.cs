using EventBus;

namespace PaymentService.API.Application.IntegrationEvents
{
    public class PaymentCompletedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
