using CourseService.Application.Dtos.Course;
using CourseService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.AuthorizationHandlers
{
    public class GetCourseAuthorizationHandler : AuthorizationHandler<GetCourseRequirement, CourseDetailDto>
    {
        private readonly ICourseService _courseService;

        public GetCourseAuthorizationHandler(ICourseService courseService)
        {
            _courseService = courseService;
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

            var enrolled = await _courseService.CheckEnrollment(course.Id, userId);
            if (enrolled)
            {
                context.Succeed(requirement);
            }
        }
    }
}
