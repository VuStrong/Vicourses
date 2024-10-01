using CourseService.Domain.Exceptions;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Quiz : Entity, IBaseEntity
    {
        private const int MaxAnswersInQuiz = 5;
        private List<Answer> _answers = [];

        public string Id { get; private set; }
        public string Title { get; private set; }
        public int Number { get; private set; }
        public bool IsMultiChoice { get; private set; }
        public string LessionId { get; private set; }
        public string UserId { get; private set; }

        public IReadOnlyList<Answer> Answers => _answers.AsReadOnly();

        private Quiz(string id, int number, string title, string lessionId, string userId)
        {
            Id = id;
            Number = number;
            Title = title;
            LessionId = lessionId;
            UserId = userId;
        }

        internal static Quiz Create(string title, int number, string lessionId, string userId, List<Answer> answers)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(title, 3, 100, nameof(title));
            DomainValidationException.ThrowIfNegative(number, nameof(number));

            var id = StringExtensions.GenerateIdString(14);
            
            var quiz = new Quiz(id, number, title, lessionId, userId);

            quiz.SetAnswers(answers);

            return quiz;
        }

        public void UpdateInfoIgnoreNull(string? title = null, List<Answer>? answers = null)
        {
            if (title != null)
            {
                title = title.Trim();
                DomainValidationException.ThrowIfStringOutOfLength(title, 3, 100, nameof(title));

                Title = title;
            }

            if (answers != null)
            {
                SetAnswers(answers);
            }
        }

        private void ClearAnswers()
        {
            _answers.Clear();
        }

        private void SetAnswers(List<Answer> answers)
        {
            int count = answers.Count;
            if (count < 2 || count > MaxAnswersInQuiz)
            {
                throw new DomainValidationException($"A quiz must have 2 to {MaxAnswersInQuiz} answers");
            }

            int correctAnswerCount = answers.Count(a => a.IsCorrect);
            if (correctAnswerCount == 0)
            {
                throw new DomainValidationException("Quiz must have at least one correct answer");
            }
            else if (correctAnswerCount == 1)
            {
                IsMultiChoice = false;
            }
            else
            {
                IsMultiChoice = true;
            }

            ClearAnswers();

            for (int index = 0; index < count; index++)
            {
                var answer = answers[index];

                answer.Number = index + 1;

                _answers.Add(answer);
            }
        }
    }

    public class Answer
    {
        public int Number { get; internal set; }
        public string Title { get; private set; }
        public bool IsCorrect { get; private set; }
        public string? Explanation { get; private set; }

        private Answer(int number, string title, bool isCorrect)
        {
            Number = number;
            Title = title;
            IsCorrect = isCorrect;
        }

        public static Answer Create(string title, bool isCorrect, string? explanation = null)
        {
            title = title.Trim();

            return new Answer(0, title, isCorrect)
            {
                Explanation = explanation
            };
        }
    }
}
