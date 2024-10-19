using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<long> CountByLessonIdAsync(string lessonId);
        Task<List<Quiz>> FindByLessonIdAsync(string lessonId);
        Task ChangeOrderAsync(string lessonId, List<string> quizIds);
        Task DeleteByLessonIdAsync(string lessonId);
        Task DeleteByLessonIdsAsync(IEnumerable<string> lessonIds);
    }
}
