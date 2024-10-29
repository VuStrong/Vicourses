using CourseService.Application.Dtos.Course;
using CourseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetCourseAuthorizationHandler : AuthorizationHandler<GetCourseRequirement, CourseDetailDto>
    {
        private readonly IEnrollService _enrollService;

        public GetCourseAuthorizationHandler(IEnrollService enrollService)
        {
            _enrollService = enrollService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetCourseRequirement requirement,
            CourseDetailDto course)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (userRole == Roles.Admin || course.Status == "Published")
            {
                context.Succeed(requirement);
                return;
            }

            if (course.User.Id == userId)
            {
                context.Succeed(requirement);
                return;
            }

            var enrolled = await _enrollService.CheckEnrollmentAsync(course.Id, userId);
            if (enrolled)
            {
                context.Succeed(requirement);
            }
        }
    }
}
