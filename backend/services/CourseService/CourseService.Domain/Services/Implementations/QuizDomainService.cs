using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;

namespace CourseService.Domain.Services.Implementations
{
    public class QuizDomainService : IQuizDomainService
    {
        private const int MaxQuizzesInLesson = 10;
        private readonly IQuizRepository _quizRepository;

        public QuizDomainService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Quiz> CreateQuizForLessonAsync(Lesson lesson, string title, List<Answer> answers)
        {
            if (lesson.Type != LessonType.Quiz)
            {
                throw new BusinessRuleViolationException("Cannot add quiz to a Non-Quiz lesson");
            }

            var quizCount = await _quizRepository.CountByLessonIdAsync(lesson.Id);
            if (quizCount >= MaxQuizzesInLesson)
            {
                throw new BusinessRuleViolationException($"A lesson can only have a maximum of {MaxQuizzesInLesson} quizzes");
            }

            var quiz = Quiz.Create(title, Convert.ToInt32(quizCount + 1), lesson, answers);

            return quiz;
        }
    }
}
