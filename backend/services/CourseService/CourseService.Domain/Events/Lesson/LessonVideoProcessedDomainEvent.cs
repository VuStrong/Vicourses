namespace CourseService.Domain.Events.Lesson
{
    public class LessonVideoProcessedDomainEvent : DomainEvent
    {
        public Models.Lesson Lesson { get; set; }

        public LessonVideoProcessedDomainEvent(Models.Lesson lesson)
        {
            Lesson = lesson;
        }
    }
}
