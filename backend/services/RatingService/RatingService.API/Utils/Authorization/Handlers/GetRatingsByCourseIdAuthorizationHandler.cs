using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RatingService.API.Infrastructure;
using RatingService.API.Models;
using System.Security.Claims;

namespace RatingService.API.Utils.Authorization.Handlers
{
    public class GetRatingsByCourseIdAuthorizationHandler : AuthorizationHandler<GetRatingsByCourseIdRequirement, string>
    {
        private readonly RatingServiceDbContext _context;

        public GetRatingsByCourseIdAuthorizationHandler(RatingServiceDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetRatingsByCourseIdRequirement requirement,
            string courseId)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            
            if (course != null)
            {
                if (course.Status == CourseStatus.Published || course.InstructorId == userId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
