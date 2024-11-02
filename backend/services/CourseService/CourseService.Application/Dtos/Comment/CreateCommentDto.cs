namespace CourseService.Application.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string LessonId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string? ReplyToId { get; set; }

        public CreateCommentDto(string lessonId, string userId, string content, string? replyToId)
        {
            LessonId = lessonId;
            UserId = userId;
            Content = content;
            ReplyToId = replyToId;
        }
    }
}
