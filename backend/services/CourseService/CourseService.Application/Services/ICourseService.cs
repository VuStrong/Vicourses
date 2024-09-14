using CourseService.Application.Dtos.Course;

namespace CourseService.Application.Services
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetCoursesAsync();
        Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId);

        Task<CourseDto> CreateCourseAsync(CreateCourseDto data);
    }
}
