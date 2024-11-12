using DiscountService.API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiscountService.API.Utils.Authorization.Handlers
{
    public class GetCouponsByCourseAuthorizationHandler : AuthorizationHandler<GetCouponsByCourseRequirement, string>
    {
        private readonly DiscountServiceDbContext _context;

        public GetCouponsByCourseAuthorizationHandler(DiscountServiceDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetCouponsByCourseRequirement requirement,
            string courseId)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course != null && course.CreatorId == userId)
            {
                context.Succeed(requirement);
            }
        }
    }
}
