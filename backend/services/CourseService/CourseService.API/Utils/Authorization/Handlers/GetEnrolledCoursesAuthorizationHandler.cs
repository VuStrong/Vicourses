using CourseService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetEnrolledCoursesAuthorizationHandler : AuthorizationHandler<GetEnrolledCoursesRequirement, User>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetEnrolledCoursesRequirement requirement,
            User user)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (user.EnrolledCoursesVisible)
            {
                context.Succeed(requirement);
            }
            else if (user.Id == userId || userRole == Roles.Admin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
