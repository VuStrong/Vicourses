using SearchService.API.Models;

namespace SearchService.API.Application.Interfaces
{
    public interface ICoursesCommandService
    {
        Task InsertOrUpdateCourseAsync(Course course, CancellationToken cancellationToken = default);

        Task DeleteCourseAsync(string courseId, CancellationToken cancellationToken = default);
    }
}
