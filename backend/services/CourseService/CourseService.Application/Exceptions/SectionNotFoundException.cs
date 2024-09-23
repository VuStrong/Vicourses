namespace CourseService.Application.Exceptions
{
    public class SectionNotFoundException : NotFoundException
    {
        public SectionNotFoundException(string id) : base("section", id) { }
    }
}
