using PaymentService.API.Application.Dtos;
using PaymentService.API.Models;

namespace PaymentService.API.Application.Mappers
{
    public static class ModelToDtoMapper
    {
        public static PaymentDto ToPaymentDto(this Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Username = payment.Username,
                Email = payment.Email,
                CourseId = payment.CourseId,
                CourseName = payment.CourseName,
                CreatedAt = payment.CreatedAt,
                PaymentDueDate = payment.PaymentDueDate,
                RefundDueDate = payment.RefundDueDate,
                ListPrice = payment.ListPrice,
                Discount = payment.Discount,
                TotalPrice = payment.TotalPrice,
                CouponCode = payment.CouponCode,
                Status = payment.Status,
                Method = payment.Method,
                PaypalOrderId = payment.PaypalOrderId,
            };
        }

        public static IEnumerable<PaymentDto> ToPaymentDtos(this IEnumerable<Payment> payments)
        {
            return payments.Select(payment =>
            {
                return new PaymentDto
                {
                    Id = payment.Id,
                    UserId = payment.UserId,
                    Username = payment.Username,
                    Email = payment.Email,
                    CourseId = payment.CourseId,
                    CourseName = payment.CourseName,
                    CreatedAt = payment.CreatedAt,
                    PaymentDueDate = payment.PaymentDueDate,
                    RefundDueDate = payment.RefundDueDate,
                    ListPrice = payment.ListPrice,
                    Discount = payment.Discount,
                    TotalPrice = payment.TotalPrice,
                    CouponCode = payment.CouponCode,
                    Status = payment.Status,
                    Method = payment.Method,
                    PaypalOrderId = payment.PaypalOrderId,
                };
            });
        }
    }
}
