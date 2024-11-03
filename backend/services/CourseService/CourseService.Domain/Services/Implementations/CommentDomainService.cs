using CourseService.Domain.Contracts;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Models;

namespace CourseService.Domain.Services.Implementations
{
    public class CommentDomainService : ICommentDomainService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CommentDomainService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Comment> CreateCommentAsync(Lesson lesson, User user, string content, Comment? replyTo)
        {
            if (user.Id != lesson.UserId)
            {
                var enrolled = await CheckUserEnrolled(lesson.CourseId, user.Id);

                if (!enrolled)
                {
                    throw new UserNotEnrolledCourseException(user.Id, lesson.CourseId);
                }
            }

            var comment = Comment.Create(lesson, user, content, replyTo);

            return comment;
        }

        private async Task<bool> CheckUserEnrolled(string courseId, string userId)
        {
            return await _enrollmentRepository.ExistsAsync(e => e.CourseId == courseId && e.UserId == userId);
        }
    }
}
