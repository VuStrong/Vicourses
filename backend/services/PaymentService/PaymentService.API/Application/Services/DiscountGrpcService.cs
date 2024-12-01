using DiscountService.Grpc;
using Grpc.Net.Client;
using PaymentService.API.Application.Dtos;
using PaymentService.API.Application.Exceptions;

namespace PaymentService.API.Application.Services
{
    public class DiscountGrpcService : IDiscountService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;

        public DiscountGrpcService(string discountGrpcAddress)
        {
            var channel = GrpcChannel.ForAddress(discountGrpcAddress);
            _client = new DiscountProtoService.DiscountProtoServiceClient(channel);
        }

        public async Task<CouponCheckResultDto> CheckCouponAsync(string code, string courseId, string userId)
        {
            var request = new CheckCouponRequest
            {
                Code = code,
                CourseId = courseId,
                UserId = userId,
            };

            var response = await _client.CheckCouponAsync(request);

            return new CouponCheckResultDto
            {
                Code = response.Code,
                CourseId = response.CourseId,
                Discount = response.Discount,
                Price = decimal.Parse(response.Price),
                DiscountPrice = decimal.Parse(response.DiscountPrice),
                Remain = response.Remain,
                IsValid = response.IsValid,
                Details = response.Details,
            };
        }

        public async Task ConsumeCouponAsync(string code, string courseId, string userId)
        {
            var request = new CheckCouponRequest
            {
                Code = code,
                CourseId = courseId,
                UserId = userId,
            };

            var response = await _client.ConsumeCouponAsync(request);

            if (!response.Success)
            {
                throw new AppException(response.Details, 422);
            }
        }
    }
}
