using CourseService.Application.Dtos.Course;
using EventBus;

namespace CourseService.Application.IntegrationEvents.Course
{
    public class UserEnrolledIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public CourseDto Course { get; set; }
        public DateOnly EnrolledAt { get; set; }

        public UserEnrolledIntegrationEvent(string userId, CourseDto course)
        {
            UserId = userId;
            Course = course;
        }
    }
}