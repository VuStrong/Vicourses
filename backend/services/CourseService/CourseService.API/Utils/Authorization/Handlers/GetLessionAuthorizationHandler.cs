using CourseService.Application.Dtos.Lession;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetLessionAuthorizationHandler : AuthorizationHandler<GetLessionRequirement, LessionDto>
    {
        private readonly ICourseService _courseService;

        public GetLessionAuthorizationHandler(ICourseService courseService)
        {
            _courseService = courseService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetLessionRequirement requirement,
            LessionDto lession)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (userRole == Roles.Admin)
            {
                context.Succeed(requirement);
                return;
            }

            var course = await _courseService.GetCourseDetailByIdAsync(lession.CourseId);

            if (userId == course.User.Id)
            {
                context.Succeed(requirement);
                return;
            }

            var enrolled = await _courseService.CheckEnrollment(course.Id, userId);
            if (enrolled)
            {
                context.Succeed(requirement);
            }
        }
    }
}
