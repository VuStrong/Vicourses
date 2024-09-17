using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Constracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<PagedResult<Course>> FindManyAsync(int skip, int limit, string? searchKeyword = null, string? categoryId = null,
            string? subCategoryId = null);

        Task IncreaseStudentCount(string courseId, int count);
    }
}
