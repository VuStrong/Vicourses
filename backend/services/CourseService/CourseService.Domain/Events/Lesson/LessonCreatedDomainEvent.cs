namespace CourseService.Domain.Events.Lesson
{
    public class LessonCreatedDomainEvent : DomainEvent
    {
        public Models.Lesson Lesson { get; set; }

        public LessonCreatedDomainEvent(Models.Lesson lesson)
        {
            Lesson = lesson;
        }
    }
}
