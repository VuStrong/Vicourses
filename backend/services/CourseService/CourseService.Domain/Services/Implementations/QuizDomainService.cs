using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;

namespace CourseService.Domain.Services.Implementations
{
    public class QuizDomainService : IQuizDomainService
    {
        private const int MaxQuizzesInLession = 10;
        private readonly IQuizRepository _quizRepository;

        public QuizDomainService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Quiz> CreateQuizForLessionAsync(Lession lession, string title, string userId, List<Answer> answers)
        {
            if (lession.Type != LessionType.Quiz)
            {
                throw new DomainException("Cannot add quiz to a Non-Quiz lession");
            }

            var quizCount = await _quizRepository.CountByLessionIdAsync(lession.Id);
            if (quizCount >= MaxQuizzesInLession)
            {
                throw new DomainException($"A lession can only have a maximum of {MaxQuizzesInLession} quizzes");
            }

            var quiz = Quiz.Create(title, Convert.ToInt32(quizCount + 1), lession.Id, userId, answers);

            return quiz;
        }
    }
}
