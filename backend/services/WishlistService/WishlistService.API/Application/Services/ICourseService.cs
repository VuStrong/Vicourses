using WishlistService.API.Models;

namespace WishlistService.API.Application.Services
{
    public interface ICourseService
    {
        Task AddOrUpdateCourseAsync(Course course);
        Task UnpublishCourseAsync(string courseId);
    }
}
