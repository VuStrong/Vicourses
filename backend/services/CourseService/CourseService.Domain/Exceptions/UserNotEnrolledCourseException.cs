namespace CourseService.Domain.Exceptions
{
    public class UserNotEnrolledCourseException : DomainException
    {
        public UserNotEnrolledCourseException(string userId, string courseId) 
            : base($"User {userId} has not enrolled course {courseId}") { }
    }
}
