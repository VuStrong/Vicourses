using CourseService.Application.IntegrationEvents.Payment;
using CourseService.Application.Interfaces;
using EventBus;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.IntegrationEventHandlers.Payment
{
    internal class PaymentRefundedIntegrationEventHandler : IIntegrationEventHandler<PaymentRefundedIntegrationEvent>
    {
        private readonly IEnrollService _enrollService;
        private readonly ILogger<PaymentRefundedIntegrationEventHandler> _logger;

        public PaymentRefundedIntegrationEventHandler(
            IEnrollService enrollService,
            ILogger<PaymentRefundedIntegrationEventHandler> logger)
        {
            _enrollService = enrollService;
            _logger = logger;
        }

        public async Task Handle(PaymentRefundedIntegrationEvent @event)
        {
            _logger.LogInformation("[Course Service] Handle PaymentRefundedIntegrationEvent: {msg}", @event.Id);

            await _enrollService.UnenrollAsync(@event.CourseId, @event.UserId);
        }
    }
}
