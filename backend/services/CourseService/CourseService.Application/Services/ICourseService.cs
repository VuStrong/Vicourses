using CourseService.Application.Dtos.Course;
using CourseService.Shared.Paging;

namespace CourseService.Application.Services
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>> GetCoursesAsync(GetCoursesParamsDto? paramsDto = null);
        Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId);

        Task<CourseDto> CreateCourseAsync(CreateCourseDto data);

        Task<CourseDto> UpdateCourseAsync(string courseId, UpdateCourseDto data, string ownerId);

        Task DeleteCourseAsync(string courseId, string ownerId);

        Task Enroll(string courseId, string userId);
        Task<bool> CheckEnrollment(string courseId, string userId);
    }
}
