using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task DeleteByCourseIdAsync(string courseId);
    }
}
