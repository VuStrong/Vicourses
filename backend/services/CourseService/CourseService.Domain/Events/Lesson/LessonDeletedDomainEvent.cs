namespace CourseService.Domain.Events.Lesson
{
    public class LessonDeletedDomainEvent : DomainEvent
    {
        public Models.Lesson Lesson { get; set; }

        public LessonDeletedDomainEvent(Models.Lesson lesson)
        {
            Lesson = lesson;
        }
    }
}
