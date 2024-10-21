using CourseService.Domain.Objects;

namespace CourseService.Domain.Events.Course
{
    public class CoursePreviewVideoUpdatedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }
        public VideoFile? OldVideo { get; set; }

        public CoursePreviewVideoUpdatedDomainEvent(Models.Course course, VideoFile? oldVideo)
        {
            Course = course;
            OldVideo = oldVideo;
        }
    }
}