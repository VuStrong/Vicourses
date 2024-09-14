namespace CourseService.Application.Exceptions
{
    public class CourseNotFoundException : NotFoundException
    {
        public CourseNotFoundException(string id) : base("course", id) { }
    }
}
