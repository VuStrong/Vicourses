using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Contracts
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<PagedResult<Enrollment>> FindByUserIdAsync(string userId, int skip, int limit);
    }
}
