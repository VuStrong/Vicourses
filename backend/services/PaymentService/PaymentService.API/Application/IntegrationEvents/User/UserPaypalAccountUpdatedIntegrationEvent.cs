using EventBus;

namespace PaymentService.API.Application.IntegrationEvents.User
{
    public class UserPaypalAccountUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public UserPaypalAccount PaypalAccount { get; set; } = null!;
    }

    public class UserPaypalAccount
    {
        public string PayerId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
