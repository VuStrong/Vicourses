using CourseService.Application.Dtos.Course;
using CourseService.Shared.Paging;

namespace CourseService.Application.Interfaces
{
    public interface IEnrollService
    {
        Task EnrollAsync(string courseId, string userId, bool throwIfCourseIsPaid = false);
        Task UnenrollAsync(string courseId, string userId);

        Task<bool> CheckEnrollmentAsync(string courseId, string userId);

        Task<PagedResult<CourseDto>> GetUserEnrolledCoursesAsync(string userId, int skip, int limit, bool onlyPublishedCourses = false);
    }
}