namespace CourseService.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }

        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.", 404)
        {
        }
    }
}
