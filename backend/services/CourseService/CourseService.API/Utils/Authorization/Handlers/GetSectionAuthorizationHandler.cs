using CourseService.Application.Dtos.Section;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetSectionAuthorizationHandler : AuthorizationHandler<GetSectionRequirement, SectionDto>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetSectionRequirement requirement,
            SectionDto section)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (userRole == Roles.Admin)
            {
                context.Succeed(requirement);
            }
            else if (userId == section.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
