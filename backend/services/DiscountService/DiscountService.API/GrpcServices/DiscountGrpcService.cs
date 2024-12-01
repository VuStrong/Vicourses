using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Application.Exceptions;
using DiscountService.API.Application.Services;
using DiscountService.Grpc;
using Grpc.Core;

namespace DiscountService.API.GrpcServices
{
    public class DiscountGrpcService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ICouponService _couponService;
        private readonly ILogger<DiscountGrpcService> _logger;

        public DiscountGrpcService(
            ICouponService couponService,
            ILogger<DiscountGrpcService> logger)
        {
            _couponService = couponService;
            _logger = logger;
        }

        public override async Task<CheckCouponResponse> CheckCoupon(CheckCouponRequest request, ServerCallContext context)
        {
            var checkCouponDto = new CheckCouponDto
            {
                Code = request.Code,
                CourseId = request.CourseId,
                UserId = request.UserId,
            };

            var result = await _couponService.CheckCouponAsync(checkCouponDto);

            return new CheckCouponResponse
            {
                Code = result.Code,
                CourseId = result.CourseId,
                Discount = result.Discount ?? 0,
                Price = result.Price?.ToString() ?? "0",
                DiscountPrice = result.DiscountPrice?.ToString() ?? "0",
                Remain = result.Remain ?? 0,
                IsValid = result.IsValid,
                Details = result.Details ?? "",
            };
        }

        public override async Task<ConsumeCouponResponse> ConsumeCoupon(CheckCouponRequest request, ServerCallContext context)
        {
            var checkCouponDto = new CheckCouponDto
            {
                Code = request.Code,
                CourseId = request.CourseId,
                UserId = request.UserId,
            };

            try
            {
                await _couponService.ConsumeCouponAsync(checkCouponDto);

                _logger.LogInformation($"Coupon {request.Code} consumed by user {request.UserId}");

                return new ConsumeCouponResponse
                {
                    Success = true,
                    Details = "",
                };
            }
            catch (AppException ex)
            {
                return new ConsumeCouponResponse
                {
                    Success = false,
                    Details = ex.Message,
                };
            }
        }
    }
}
