using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface ILessionRepository : IRepository<Lession>
    {
        Task<List<Lession>> FindBySectionIdAsync(string sectionId);
        Task<List<Lession>> FindByCourseIdAsync(string courseId);

        Task DeleteBySectionIdAsync(string sectionId);
        Task DeleteByCourseIdAsync(string courseId);
    }
}
