using CourseService.Domain.Contracts;
using CourseService.Domain.Enums;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;
using CourseService.Domain.Services.Implementations;
using NSubstitute;

namespace CourseService.Tests.UnitTests.Domain
{
    public class QuizModelTest
    {
        private Lesson _lesson;
        private IQuizRepository _quizRepository;

        public QuizModelTest()
        {
            _lesson = (Lesson)Activator.CreateInstance(typeof(Lesson), true)!;
            typeof(Lesson).GetProperty("Id")!.SetValue(_lesson, "les1", null);
            typeof(Lesson).GetProperty("Type")!.SetValue(_lesson, LessonType.Quiz, null);

            _quizRepository = Substitute.For<IQuizRepository>();
        }

        [Fact]
        public async Task CreateQuizForLessonAsync_ShouldThrow_WhenLessonTypeIsNotQuiz()
        {
            typeof(Lesson).GetProperty("Type")!.SetValue(_lesson, LessonType.Video, null);

            _quizRepository.CountByLessonIdAsync(_lesson.Id).Returns(1);

            var quizDomainService = new QuizDomainService(_quizRepository);

            var answers = new List<Answer>()
            {
                Answer.Create("ans1", true, null),
                Answer.Create("ans2", false, null),
            };

            await Assert.ThrowsAsync<DomainException>(() => quizDomainService.CreateQuizForLessonAsync(_lesson, "quiz1", answers));
        }

        [Fact]
        public async Task CreateQuizForLessonAsync_ShouldThrow_WhenExceedMaximumOfQuizzesInLesson()
        {
            _quizRepository.CountByLessonIdAsync(_lesson.Id).Returns(11);

            var quizDomainService = new QuizDomainService(_quizRepository);

            var answers = new List<Answer>()
            {
                Answer.Create("ans1", true, null),
                Answer.Create("ans2", false, null),
            };

            await Assert.ThrowsAsync<DomainException>(() => quizDomainService.CreateQuizForLessonAsync(_lesson, "quiz1", answers));
        }

        [Fact]
        public async Task CreateQuizForLessonAsync_ShouldThrow_WhenNotHaveCorrectAnswer()
        {
            _quizRepository.CountByLessonIdAsync(_lesson.Id).Returns(1);

            var quizDomainService = new QuizDomainService(_quizRepository);

            var answers = new List<Answer>()
            {
                Answer.Create("ans1", false, null),
                Answer.Create("ans2", false, null),
                Answer.Create("ans3", false, null),
            };

            await Assert.ThrowsAsync<DomainValidationException>(() => quizDomainService.CreateQuizForLessonAsync(_lesson, "quiz1", answers));
        }

        [Fact]
        public async Task CreateQuizForLessonAsync_ShouldThrow_WhenExceedMaximumOfAnsersInQuiz()
        {
            _quizRepository.CountByLessonIdAsync(_lesson.Id).Returns(1);

            var quizDomainService = new QuizDomainService(_quizRepository);

            var answers = new List<Answer>()
            {
                Answer.Create("ans1", true, null),
                Answer.Create("ans2", false, null),
                Answer.Create("ans3", false, null),
                Answer.Create("ans3", false, null),
                Answer.Create("ans3", false, null),
                Answer.Create("ans3", false, null),
            };

            await Assert.ThrowsAsync<DomainValidationException>(() => quizDomainService.CreateQuizForLessonAsync(_lesson, "quiz1", answers));
        }
    }
}
