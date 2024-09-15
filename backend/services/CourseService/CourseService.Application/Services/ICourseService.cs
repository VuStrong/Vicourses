using CourseService.Application.Dtos.Course;

namespace CourseService.Application.Services
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetCoursesAsync();
        Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId);

        Task<CourseDto> CreateCourseAsync(CreateCourseDto data);

        Task<CourseDto> UpdateCourseAsync(string courseId, UpdateCourseDto data, string ownerId);

        Task DeleteCourseAsync(string courseId, string ownerId);

        Task Enroll(string courseId, string userId);
        Task<bool> CheckEnrollment(string courseId, string userId);
    }
}
