using CourseService.Domain.Models;

namespace CourseService.Domain.Constracts
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task IncreaseStudentCount(string courseId, int count);

    }
}
