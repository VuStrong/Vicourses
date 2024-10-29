using CourseService.Application.Dtos.Lesson;
using CourseService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CourseService.API.Utils.Authorization.Handlers
{
    public class GetLessonAuthorizationHandler : AuthorizationHandler<GetLessonRequirement, LessonDto>
    {
        private readonly IEnrollService _enrollService;

        public GetLessonAuthorizationHandler(IEnrollService enrollService)
        {
            _enrollService = enrollService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetLessonRequirement requirement,
            LessonDto lesson)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (userRole == Roles.Admin)
            {
                context.Succeed(requirement);
                return;
            }

            if (userId == lesson.UserId)
            {
                context.Succeed(requirement);
                return;
            }

            var enrolled = await _enrollService.CheckEnrollmentAsync(lesson.CourseId, userId);
            if (enrolled)
            {
                context.Succeed(requirement);
            }
        }
    }
}
