using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface IQuizDomainService
    {
        Task<Quiz> CreateQuizForLessonAsync(Lesson lesson, string title, string userId, List<Answer> answers);
    }
}
