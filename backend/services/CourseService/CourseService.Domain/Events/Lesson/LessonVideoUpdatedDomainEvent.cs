using CourseService.Domain.Objects;

namespace CourseService.Domain.Events.Lesson
{
    public class LessonVideoUpdatedDomainEvent : DomainEvent
    {
        public Models.Lesson Lesson { get; set; }
        public VideoFile? OldVideo { get; set; }

        public LessonVideoUpdatedDomainEvent(Models.Lesson lesson, VideoFile? oldVideo)
        {
            Lesson = lesson;
            OldVideo = oldVideo;
        }
    }
}