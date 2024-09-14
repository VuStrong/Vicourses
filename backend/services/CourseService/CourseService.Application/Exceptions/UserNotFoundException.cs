namespace CourseService.Application.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) : base("user", id) { }
    }
}
