using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
        Task InsertCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task UpdateStatusAsync(string courseId, CourseStatus status);
    }
}
