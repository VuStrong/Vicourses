using CourseService.Domain.Objects;

namespace CourseService.Domain.Events.Course
{
    public class CourseThumbnailUpdatedDomainEvent : DomainEvent
    {
        public Models.Course Course { get; set; }
        public ImageFile? OldThumbnail { get; set; }

        public CourseThumbnailUpdatedDomainEvent(Models.Course course, ImageFile? oldThumbnail)
        {
            Course = course;
            OldThumbnail = oldThumbnail;
        }
    }
}