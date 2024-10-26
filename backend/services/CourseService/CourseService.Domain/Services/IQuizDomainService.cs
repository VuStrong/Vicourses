using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface IQuizDomainService
    {
        Task<Quiz> CreateQuizForLessonAsync(Lesson lesson, string title, List<Answer> answers);
    }
}
