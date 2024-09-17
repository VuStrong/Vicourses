using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Constracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<PagedResult<Course>> FindManyAsync(int skip, int limit, CourseSort? sort = null, string? searchKeyword = null, 
            string? categoryId = null, string? subCategoryId = null, bool? isPaid = null, CourseLevel? level = null, 
            decimal? minimumRating = null);

        Task IncreaseStudentCount(string courseId, int count);
    }
}
