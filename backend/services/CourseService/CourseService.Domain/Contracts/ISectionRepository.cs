using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<long> CountByCourseIdAsync(string courseId);
    }
}
