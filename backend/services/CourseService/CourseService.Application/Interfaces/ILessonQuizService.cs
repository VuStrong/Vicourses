using CourseService.Application.Dtos.Quiz;

namespace CourseService.Application.Interfaces
{
    public interface ILessonQuizService
    {
        Task<List<QuizDto>> GetQuizzesByLessonIdAsync(string lessonId);
        Task<QuizDto> CreateLessonQuizAsync(CreateLessonQuizDto data);
        Task<QuizDto> UpdateLessonQuizAsync(string quizId, UpdateLessonQuizDto data, string ownerId);
        Task DeleteLessonQuizAsync(string quizId, string ownerId);
        Task ChangeOrderOfQuizzesAsync(string lessonId, List<string> quizIds, string ownerId);
    }
}
