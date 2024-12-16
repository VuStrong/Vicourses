using CourseService.Domain.Exceptions;

namespace CourseService.Domain.Objects
{
    public class Quiz
    {
        private const int MaxAnswersInQuiz = 5;
        private List<Answer> _answers = [];

        public int Number { get; internal set; }
        public string Title { get; private set; }
        public bool IsMultiChoice { get; private set; }

        public IReadOnlyList<Answer> Answers => _answers.AsReadOnly();

        private Quiz(int number, string title)
        {
            Number = number;
            Title = title;
        }

        internal static Quiz Create(int number, string title, List<Answer> answers)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfNegative(number, nameof(number));
            DomainValidationException.ThrowIfStringNullOrEmpty(title, nameof(title));

            var quiz = new Quiz(number, title);

            quiz.SetAnswers(answers);

            return quiz;
        }

        public void UpdateInfoIgnoreNull(string? title = null, List<Answer>? answers = null)
        {
            if (title != null)
            {
                title = title.Trim();
                DomainValidationException.ThrowIfStringNullOrEmpty(title, nameof(title));

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
