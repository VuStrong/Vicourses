using CourseService.Domain.Exceptions;

namespace CourseService.Domain.Models
{
    public record UserInComment(string Id, string Name, string? ThumbnailUrl);

    public class Comment : Entity, IBaseEntity
    {
        private List<string> _userUpvoteIds = [];

        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string LessonId { get; private set; }
        public UserInComment User { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public string? ReplyToId { get; private set; }
        public int UpvoteCount { get; private set; }

        public IReadOnlyList<string> UserUpvoteIds => _userUpvoteIds.AsReadOnly();

        public bool IsRoot { get => ReplyToId == null; }

        private Comment(string id, string courseId, string lessonId, UserInComment user, string content)
        {
            Id = id;
            CourseId = courseId;
            LessonId = lessonId;
            User = user;
            Content = content;
        }

        public static Comment Create(Lesson lesson, User user, string content, Comment? replyTo)
        {
            content = content.Trim();
            DomainValidationException.ThrowIfStringNullOrEmpty(content, nameof(content));

            if (replyTo != null)
            {
                if (!replyTo.IsRoot)
                {
                    throw new DomainException("ReplyTo is not root comment");
                }

                if (replyTo.IsDeleted)
                {
                    throw new DomainException("ReplyTo is deleted comment");
                }
            }

            var id = Guid.NewGuid().ToString();
            var userInComment = new UserInComment(user.Id, user.Name, user.ThumbnailUrl);

            return new Comment(id, lesson.CourseId, lesson.Id, userInComment, content)
            {
                CreatedAt = DateTime.Now,
                ReplyToId = replyTo?.Id,
            };
        }

        public void SetDeleted()
        {
            if (IsDeleted) return;

            IsDeleted = true;

            _userUpvoteIds.Clear();
            Content = string.Empty;
            UpvoteCount = 0;
        }

        public void Upvote(string userId)
        {
            if (IsDeleted)
            {
                throw new DomainException("Cannot upvote deleted comment");
            }

            if (_userUpvoteIds.Contains(userId)) return;

            _userUpvoteIds.Add(userId);
            UpvoteCount++;
        }

        public void RemoveUpvote(string userId)
        {
            if (IsDeleted)
            {
                throw new DomainException("Cannot remove upvote of deleted comment");
            }

            _userUpvoteIds.Remove(userId);
            UpvoteCount--;
        }
    }
}
