namespace CourseService.Application.Dtos.Comment
{
    public record UserInCommentDto(string Id, string Name, string? ThumbnailUrl);

    public class CommentDto
    {
        public string Id { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string LessonId { get; set; } = string.Empty;
        public UserInCommentDto User { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? ReplyToId { get; set; }
        public int UpvoteCount { get; set; }
        public List<string> UserUpvoteIds { get; set; } = [];
    }
}
