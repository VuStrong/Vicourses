using EventBus;

namespace CourseService.Application.IntegrationEvents.Payment
{
    public class PaymentCompletedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}