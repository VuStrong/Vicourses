using EventBus;

namespace CourseService.Application.IntegrationEvents.Payment
{
    internal class PaymentRefundedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }
}
