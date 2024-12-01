using PaymentService.API.Application.IntegrationEvents;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Mappers
{
    public static class ModelToIntegrationEventMapper
    {
        public static PaymentCompletedIntegrationEvent ToPaymentCompletedIntegrationEvent(this Payment payment)
        {
            return new PaymentCompletedIntegrationEvent
            {
                Id = payment.Id,
                UserId = payment.UserId,
                CourseId = payment.CourseId,
                TotalPrice = payment.TotalPrice,
                CreatedAt = payment.CreatedAt,
            };
        }

        public static PaymentRefundedIntegrationEvent ToPaymentRefundedIntegrationEvent(this Payment payment)
        {
            return new PaymentRefundedIntegrationEvent
            {
                Id = payment.Id,
                UserId = payment.UserId,
                CourseId = payment.CourseId,
                Reason = payment.RefundReason,
            };
        }
    }
}
