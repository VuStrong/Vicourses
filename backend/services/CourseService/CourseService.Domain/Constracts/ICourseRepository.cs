using CourseService.Domain.Models;

namespace CourseService.Domain.Constracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task UpdateCategoryInCourses(Category category);
    }
}
