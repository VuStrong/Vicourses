using EventBus;

namespace PaymentService.API.Application.IntegrationEvents.User
{
    public class UserInfoUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
