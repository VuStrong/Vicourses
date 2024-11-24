using CourseService.Application.IntegrationEvents.Payment;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.Payment
{
    public class PaymentCompletedIntegrationEventHandler : IIntegrationEventHandler<PaymentCompletedIntegrationEvent>
    {
        private readonly IEnrollService _enrollService;
        private readonly ILogger<PaymentCompletedIntegrationEventHandler> _logger;

        public PaymentCompletedIntegrationEventHandler(
            IEnrollService enrollService,
            ILogger<PaymentCompletedIntegrationEventHandler> logger
        )
        {
            _enrollService = enrollService;
            _logger = logger;
        }

        public async Task Handle(PaymentCompletedIntegrationEvent @event)
        {
            _logger.LogInformation($"[Course Service] Handle PaymentCompletedIntegrationEvent: {@event.Id}");
            
            await _enrollService.EnrollAsync(@event.CourseId, @event.UserId);
        }
    }
}