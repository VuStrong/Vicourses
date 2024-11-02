using CourseService.Application.Dtos.Comment;
using CourseService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Comment
{
    public class GetCommentsRequest : PagingRequest
    {
        public string? ReplyToId { get; set; }

        [EnumDataType(typeof(CommentSort))]
        public CommentSort Sort { get; set; } = CommentSort.Newest;

        public GetCommentsParamsDto ToGetCommentsParamsDto(string lessonId)
        {
            return new GetCommentsParamsDto()
            {
                Skip = Skip,
                Limit = Limit,
                LessonId = lessonId,
                ReplyToId = ReplyToId,
                Sort = Sort
            };
        }
    }
}
