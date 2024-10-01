using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface IQuizDomainService
    {
        Task<Quiz> CreateQuizForLessionAsync(Lession lession, string title, string userId, List<Answer> answers);
    }
}
