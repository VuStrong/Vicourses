namespace CourseService.Application.Exceptions
{
    public class LessionNotFoundException : NotFoundException
    {
        public LessionNotFoundException(string id) : base("lession", id) { }
    }
}
