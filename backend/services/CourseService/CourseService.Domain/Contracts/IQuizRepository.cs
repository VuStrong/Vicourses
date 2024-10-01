using CourseService.Domain.Models;

namespace CourseService.Domain.Contracts
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<long> CountByLessionIdAsync(string lessionId);
        Task<List<Quiz>> FindByLessionIdAsync(string lessionId);
        Task ChangeOrderAsync(string lessionId, List<string> quizIds);
        Task DeleteByLessionIdAsync(string lessionId);
    }
}
