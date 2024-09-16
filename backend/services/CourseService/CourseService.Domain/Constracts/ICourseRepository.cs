using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Constracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<PagedResult<Course>> FindManyAsync(int skip, int limit, string? searchKeyword = null);

        Task IncreaseStudentCount(string courseId, int count);
    }
}
