using EventBus;

namespace PaymentService.API.Application.IntegrationEvents
{
    public class SendEmailIntegrationEvent : IntegrationEvent
    {
        public string To { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public object Payload { get; set; } = null!;
    }
}
