using CourseService.Domain.Enums;
using CourseService.Domain.Models;
using CourseService.Shared.Paging;

namespace CourseService.Domain.Contracts
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<PagedResult<Comment>> FindByLessonIdAsync(string lessonId, int skip, int limit, CommentSort? sort = null,
            string? replyToId = null, CancellationToken cancellationToken = default);
    
        Task UpdateUserInCommentsAsync(UserInComment user);
    }
}
