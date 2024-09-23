using CourseService.Application.Dtos.Section;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetSectionAuthorizationHandler : AuthorizationHandler<GetSectionRequirement, SectionDto>
    {
        private readonly ICourseService _courseService;

        public GetSectionAuthorizationHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetSectionRequirement requirement,
            SectionDto section)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (userRole == Roles.Admin)
            {
                context.Succeed(requirement);
                return;
            }

            var course = await _courseService.GetCourseDetailByIdAsync(section.CourseId);

            if (userId == course.User.Id)
            {
                context.Succeed(requirement);
            }
        }
    }
}
