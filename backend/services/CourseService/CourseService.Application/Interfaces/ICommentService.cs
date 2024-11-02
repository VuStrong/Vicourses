using CourseService.Application.Dtos.Comment;
using CourseService.Shared.Paging;

namespace CourseService.Application.Interfaces
{
    public interface ICommentService
    {
        Task<PagedResult<CommentDto>> GetCommentsAsync(GetCommentsParamsDto paramsDto, CancellationToken cancellationToken = default);
        Task<CommentDto> CreateCommentAsync(CreateCommentDto data);
        Task UpvoteCommentAsync(string commentId, string userId);
        Task CancelUpvoteAsync(string commentId, string userId);
        Task DeleteCommentAsync(string commentId, string userId);
    }
}
