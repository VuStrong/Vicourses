using CourseService.Domain.Enums;

namespace CourseService.Application.Dtos.Comment
{
    public class GetCommentsParamsDto
    {
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 10;
        public required string LessonId { get; set; }
        public string? ReplyToId { get; set; }
        public CommentSort Sort { get; set; } = CommentSort.Newest;
    }
}
