using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Contracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<PagedResult<Course>> FindManyAsync(int skip, int limit, CourseSort? sort = null, string? searchKeyword = null, 
            string? categoryId = null, string? subCategoryId = null, bool? isPaid = null, CourseLevel? level = null, 
            decimal? minimumRating = null, CourseStatus status = CourseStatus.Published, string? tag = null);

        Task<PagedResult<Course>> FindManyByUserIdAsync(string userId, int skip, int limit, string? searchKeyword = null,
            CourseStatus? status = null);

        Task UpdateStudentCountAsync(Course course);

        Task UpdateUserInCoursesAsync(UserInCourse user);
    }
}
