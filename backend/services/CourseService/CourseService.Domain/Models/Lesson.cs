using CourseService.Domain.Enums;
using CourseService.Domain.Events.Lesson;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Objects;
using CourseService.Shared.Extensions;

namespace CourseService.Domain.Models
{
    public class Lesson : Entity, IBaseEntity
    {
        private const int MaxQuizzesInLesson = 10;
        private List<Quiz> _quizzes = [];

        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string SectionId { get; private set; }
        public string UserId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Order {  get; private set; }
        public LessonType Type { get; private set; } = LessonType.Video;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public VideoFile? Video { get; private set; }
        public int QuizzesCount { get; private set; }

        public IReadOnlyList<Quiz> Quizzes => _quizzes.AsReadOnly();

        #pragma warning disable CS8618
        private Lesson() { }

        private Lesson(string id, string title, string courseId, string sectionId, string userId)
        {
            Id = id;
            Title = title;
            CourseId = courseId;
            SectionId = sectionId;
            UserId = userId;

            AddUniqueDomainEvent(new LessonCreatedDomainEvent(this));
        }

        public static Lesson Create(string title, Course course, Section section, LessonType type, string? description)
        {
            title = title.Trim();
            DomainValidationException.ThrowIfStringOutOfLength(title, 3, 80, nameof(title));

            if (section.CourseId != course.Id)
            {
                throw new BusinessRuleViolationException($"Section {section.Id} must be asset of the course {course.Id}");
            }

            var id = StringExtensions.GenerateIdString(14);

            return new Lesson(id, title, course.Id, section.Id, course.User.Id)
            {
                Description = description,
                Type = type,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }

        public void UpdateInfoIgnoreNull(string? title = null, string? description = null)
        {
            if (title != null)
            {
                title = title.Trim();
                DomainValidationException.ThrowIfStringOutOfLength(title, 3, 80, nameof(title));

                Title = title;
            }

            if (description != null) Description = description;

            UpdatedAt = DateTime.Now;
        }

        public void UpdateVideo(VideoFile video)
        {
            if (Type != LessonType.Video)
            {
                throw new BusinessRuleViolationException("Cannot update video of non-video lesson");
            }

            if (Video != null && Video.FileId == video.FileId) return;

            var oldVideo = Video;
            Video = video;

            UpdatedAt = DateTime.Now;
            
            AddUniqueDomainEvent(new LessonVideoUpdatedDomainEvent(this, oldVideo));
        }

        public void SetVideoStatusCompleted(string streamFileUrl, int duration)
        {
            if (Video == null)
            {
                throw new BusinessRuleViolationException("Cannot set video status because video is not set");
            }

            DomainValidationException.ThrowIfNegative(duration, nameof(duration));

            Video = VideoFile.Create(
                Video.FileId,
                Video.Url,
                Video.OriginalFileName,
                streamFileUrl,
                duration,
                VideoStatus.Processed
            );

            AddUniqueDomainEvent(new LessonVideoProcessedDomainEvent(this));
        }

        public void SetVideoStatusFailed()
        {
            if (Video == null)
            {
                throw new BusinessRuleViolationException("Cannot set video status because video is not set");
            }

            Video = VideoFile.Create(
                Video.FileId,
                Video.Url,
                Video.OriginalFileName,
                status: VideoStatus.ProcessingFailed
            );
        }

        public void AddQuiz(string title, List<Answer> answers)
        {
            if (Type != LessonType.Quiz)
            {
                throw new BusinessRuleViolationException("Cannot add quiz to a Non-Quiz lesson");
            }
            if (QuizzesCount >= MaxQuizzesInLesson)
            {
                throw new BusinessRuleViolationException($"A lesson can only have a maximum of {MaxQuizzesInLesson} quizzes");
            }

            _quizzes.Add(Quiz.Create(QuizzesCount + 1, title, answers));

            QuizzesCount++;
        }

        public void UpdateQuiz(int number, string title, List<Answer> answers)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Number == number);

            if (quiz == null)
            {
                throw new DomainValidationException($"Cannot find quiz with number = {number}");
            }

            quiz.UpdateInfoIgnoreNull(title, answers);
        }

        public void RemoveQuiz(int number)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Number == number);

            if (quiz == null)
            {
                throw new DomainValidationException($"Cannot find quiz with number = {number}");
            }

            _quizzes.Remove(quiz);

            foreach (var otherQuiz in _quizzes)
            {
                if (otherQuiz.Number > number)
                {
                    otherQuiz.Number--;
                }
            }

            QuizzesCount--;
        }

        /// <summary>
        /// Move a quiz to another position
        /// </summary>
        /// <param name="number">Number of the quiz</param>
        /// <param name="to">Position to move (start from 1)</param>
        public void MoveQuiz(int number, int to)
        {
            if (number == to) return;

            var quiz = _quizzes.FirstOrDefault(q => q.Number == number);

            if (quiz == null)
            {
                throw new DomainValidationException($"Cannot find quiz with number = {number}");
            }

            if (to < 1 || to > QuizzesCount)
            {
                throw new DomainValidationException($"The value {to} of 'to' is out of range");
            }

            _quizzes.Remove(quiz);
            _quizzes.Insert(to - 1, quiz);

            for (int i = 0; i < QuizzesCount; i++)
            {
                _quizzes[i].Number = i + 1;
            }
        }

        public void SetDeleted()
        {
            AddUniqueDomainEvent(new LessonDeletedDomainEvent(this));
        }
    }
}
