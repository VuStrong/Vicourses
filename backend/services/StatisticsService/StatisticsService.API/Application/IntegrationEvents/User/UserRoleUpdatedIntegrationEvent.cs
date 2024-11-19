using EventBus;
using StatisticsService.API.Utils;

namespace StatisticsService.API.Application.IntegrationEvents.User
{
    public class UserRoleUpdatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = string.Empty;
        public string Role { get; set; } = Roles.Student;
    }
}
