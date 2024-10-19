using AutoMapper;
using CourseService.Application.Dtos.Quiz;
using CourseService.Application.Exceptions;
using CourseService.Application.Interfaces;
using CourseService.Domain.Contracts;
using CourseService.Domain.Models;
using CourseService.Domain.Services;

namespace CourseService.Application.Services
{
    public class LessonQuizService : ILessonQuizService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizDomainService _quizDomainService;
        private readonly IMapper _mapper;

        public LessonQuizService(
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IQuizDomainService quizDomainService,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _quizDomainService = quizDomainService;
            _mapper = mapper;
        }

        public async Task<List<QuizDto>> GetQuizzesByLessonIdAsync(string lessonId)
        {
            var quizzes = await _quizRepository.FindByLessonIdAsync(lessonId);

            return _mapper.Map<List<QuizDto>>(quizzes);
        }

        public async Task<QuizDto> CreateLessonQuizAsync(CreateLessonQuizDto data)
        {
            var lesson = await GetAndValidateLessonAsync(data.LessonId, data.UserId);

            var answers = new List<Answer>();
            foreach (var answerDto in data.Answers)
            {
                answers.Add(Answer.Create(answerDto.Title, answerDto.IsCorrect, answerDto.Explanation));
            }

            var quiz = await _quizDomainService.CreateQuizForLessonAsync(lesson, data.Title, data.UserId, answers);

            await _quizRepository.CreateAsync(quiz);

            return _mapper.Map<QuizDto>(quiz);
        }

        public async Task<QuizDto> UpdateLessonQuizAsync(string quizId, UpdateLessonQuizDto data, string ownerId)
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

        public async Task DeleteLessonQuizAsync(string quizId, string ownerId)
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

        public async Task ChangeOrderOfQuizzesAsync(string lessonId, List<string> quizIds, string ownerId)
        {
            await GetAndValidateLessonAsync(lessonId, ownerId);

            await _quizRepository.ChangeOrderAsync(lessonId, quizIds);
        }

        private async Task<Lesson> GetAndValidateLessonAsync(string lessonId, string ownerId)
        {
            var lesson = await _lessonRepository.FindOneAsync(lessonId);

            if (lesson == null)
            {
                throw new LessonNotFoundException(lessonId);
            }

            if (lesson.UserId != ownerId)
            {
                throw new ForbiddenException("Forbidden resourse");
            }

            return lesson;
        }
    }
}
