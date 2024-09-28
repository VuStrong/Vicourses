using AutoMapper;
using CourseService.Application.Dtos.Quiz;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Models;

namespace CourseService.Application.Services
{
    public class LessionQuizService : ILessionQuizService
    {
        private const int MaxQuizzesInLession = 10;

        private readonly ICourseCurriculumManager _courseCurriculumManager;
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public LessionQuizService(
            ICourseCurriculumManager courseCurriculumManager,
            IQuizRepository quizRepository,
            IMapper mapper)
        {
            _courseCurriculumManager = courseCurriculumManager;
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<List<QuizDto>> GetQuizzesByLessionIdAsync(string lessionId)
        {
            var quizzes = await _quizRepository.FindByLessionIdAsync(lessionId);

            return _mapper.Map<List<QuizDto>>(quizzes);
        }

        public async Task<QuizDto> CreateLessionQuizAsync(CreateLessionQuizDto data)
        {
            var lession = await GetAndValidateLessionAsync(data.LessionId, data.UserId);

            if (lession.Type != LessionType.Quiz)
            {
                throw new ForbiddenException("Cannot add quiz to a Non-Quiz lession");
            }

            var quizCount = await _quizRepository.CountByLessionIdAsync(lession.Id);
            if (quizCount >= MaxQuizzesInLession)
            {
                throw new BadRequestException($"A lession can only have a maximum of {MaxQuizzesInLession} quizzes");
            }

            var quiz = Quiz.Create(data.Title, Convert.ToInt32(quizCount + 1), lession.Id, data.UserId, data.IsMultiChoice);

            foreach (var answerDto in data.Answers)
            {
                var answer = Answer.Create(answerDto.Title, answerDto.IsCorrect, answerDto.Explanation);

                quiz.AddAnswer(answer);
            }

            await _quizRepository.CreateAsync(quiz);

            return _mapper.Map<QuizDto>(quiz);
        }

        public async Task<QuizDto> UpdateLessionQuizAsync(string quizId, UpdateLessionQuizDto data, string ownerId)
        {
            var quiz = await _quizRepository.FindOneAsync(quizId);

            if (quiz == null)
            {
                throw new NotFoundException("quiz", quizId);
            }

            if (quiz.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            quiz.UpdateInfo(data.Title, data.IsMultiChoice);

            quiz.ClearAnswers();

            foreach (var answerDto in data.Answers)
            {
                var answer = Answer.Create(answerDto.Title, answerDto.IsCorrect, answerDto.Explanation);

                quiz.AddAnswer(answer);
            }

            await _quizRepository.UpdateAsync(quiz);

            return _mapper.Map<QuizDto>(quiz);
        }

        public async Task DeleteLessionQuizAsync(string quizId, string ownerId)
        {
            var quiz = await _quizRepository.FindOneAsync(quizId);

            if (quiz == null)
            {
                throw new NotFoundException("quiz", quizId);
            }

            if (quiz.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            await _quizRepository.DeleteOneAsync(quizId);
        }

        public async Task ChangeOrderOfQuizzesAsync(string lessionId, List<string> quizIds, string ownerId)
        {
            await GetAndValidateLessionAsync(lessionId, ownerId);

            await _quizRepository.ChangeOrderAsync(lessionId, quizIds);
        }

        private async Task<Lession> GetAndValidateLessionAsync(string lessionId, string ownerId)
        {
            var lession = await _courseCurriculumManager.GetLessionByIdAsync(lessionId);

            if (lession == null)
            {
                throw new LessionNotFoundException(lessionId);
            }

            if (lession.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            return lession;
        }
    }
}
