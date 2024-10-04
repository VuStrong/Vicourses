using CourseService.Application.Dtos.Course;
using CourseService.Domain.Enums;
using CourseService.Shared.Paging;

namespace CourseService.Application.Interfaces
{
    public interface ICourseService
    {
        Task<PagedResult<CourseDto>> GetCoursesAsync(GetCoursesParamsDto? paramsDto = null);
        Task<PagedResult<CourseDto>> GetCoursesByUserIdAsync(string userId, int skip, int limit, string? searchKeyword = null,
            CourseStatus? status = null);
        Task<CourseDetailDto> GetCourseDetailByIdAsync(string courseId);

        Task<CourseDto> CreateCourseAsync(CreateCourseDto data);

        Task<CourseDto> UpdateCourseAsync(string courseId, UpdateCourseDto data, string ownerId);

        Task DeleteCourseAsync(string courseId, string ownerId);

        Task ApproveCourseAsync(string courseId);
        Task CancelCourseApprovalAsync(string courseId, List<string> reasons);

        Task Enroll(string courseId, string userId);
        Task<bool> CheckEnrollment(string courseId, string userId);
    }
}
