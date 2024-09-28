using CourseService.Application.Dtos.Quiz;

namespace CourseService.Application.Interfaces
{
    public interface ILessionQuizService
    {
        Task<List<QuizDto>> GetQuizzesByLessionIdAsync(string lessionId);
        Task<QuizDto> CreateLessionQuizAsync(CreateLessionQuizDto data);
        Task<QuizDto> UpdateLessionQuizAsync(string quizId, UpdateLessionQuizDto data, string ownerId);
        Task DeleteLessionQuizAsync(string quizId, string ownerId);
        Task ChangeOrderOfQuizzesAsync(string lessionId, List<string> quizIds, string ownerId);
    }
}
