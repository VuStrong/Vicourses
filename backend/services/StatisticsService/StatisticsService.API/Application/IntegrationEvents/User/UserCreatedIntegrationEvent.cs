﻿using EventBus;

namespace StatisticsService.API.Application.IntegrationEvents.User
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
