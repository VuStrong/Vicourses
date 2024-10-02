using AutoMapper;
using CourseService.Application.Dtos.Quiz;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.Domain.Services;

namespace CourseService.Application.Services
{
    public class LessionQuizService : ILessionQuizService
    {
        private readonly ILessionRepository _lessionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizDomainService _quizDomainService;
        private readonly IMapper _mapper;

        public LessionQuizService(
            ILessionRepository lessionRepository,
            IQuizRepository quizRepository,
            IQuizDomainService quizDomainService,
            IMapper mapper)
        {
            _lessionRepository = lessionRepository;
            _quizRepository = quizRepository;
            _quizDomainService = quizDomainService;
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

            var answers = new List<Answer>();
            foreach (var answerDto in data.Answers)
            {
                answers.Add(Answer.Create(answerDto.Title, answerDto.IsCorrect, answerDto.Explanation));
            }

            var quiz = await _quizDomainService.CreateQuizForLessionAsync(lession, data.Title, data.UserId, answers);

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

            var answers = new List<Answer>();
            foreach (var answerDto in data.Answers)
            {
                answers.Add(Answer.Create(answerDto.Title, answerDto.IsCorrect, answerDto.Explanation));
            }

            quiz.UpdateInfoIgnoreNull(data.Title, answers);
            
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
            var lession = await _lessionRepository.FindOneAsync(lessionId);

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
