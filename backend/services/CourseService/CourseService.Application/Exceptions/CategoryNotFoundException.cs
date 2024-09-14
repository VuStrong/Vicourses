namespace CourseService.Application.Exceptions
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string id) : base("category", id) { }
    }
}
