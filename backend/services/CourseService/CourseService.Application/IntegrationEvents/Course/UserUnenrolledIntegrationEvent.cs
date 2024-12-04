using CourseService.Application.Dtos.Course;
using EventBus;

namespace CourseService.Application.IntegrationEvents.Course
{
    public class UserUnenrolledIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public CourseDto Course { get; set; }
        public DateOnly UnenrolledAt { get; set; }

        public UserUnenrolledIntegrationEvent(string userId, CourseDto course, DateOnly unenrolledAt)
        {
            UserId = userId;
            Course = course;
            UnenrolledAt = unenrolledAt;
        }
    }
}
