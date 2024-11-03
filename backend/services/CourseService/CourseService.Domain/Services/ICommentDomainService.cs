using CourseService.Domain.Models;

namespace CourseService.Domain.Services
{
    public interface ICommentDomainService
    {
        Task<Comment> CreateCommentAsync(Lesson lesson, User user, string content, Comment? replyTo);
    }
}
