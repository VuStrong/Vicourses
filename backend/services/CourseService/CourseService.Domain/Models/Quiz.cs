using CourseService.Domain.Contracts;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Quiz : IBaseEntity
    {
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

        public static Quiz Create(string title, int number, string lessionId, string userId, bool IsMultiChoice = false)
        {
            var id = StringExtensions.GenerateIdString(14);
            return new Quiz(id, number, title, lessionId, userId)
            {
                IsMultiChoice = IsMultiChoice
            };
        }

        public void UpdateInfo(string title, bool isMultiChoice)
        {
            Title = title;
            IsMultiChoice = isMultiChoice;
        }

        public void AddAnswer(Answer answer)
        {
            int number = _answers.Count > 0 ? _answers.Max(x => x.Number) + 1 : 1;
            answer.Number = number;

            _answers.Add(answer);
        }

        public void ClearAnswers()
        {
            _answers.Clear();
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
            return new Answer(0, title, isCorrect)
            {
                Explanation = explanation
            };
        }
    }
}
