namespace CourseService.Application.Exceptions
{
    public class LessonNotFoundException : NotFoundException
    {
        public LessonNotFoundException(string id) : base("lesson", id) { }
    }
}
