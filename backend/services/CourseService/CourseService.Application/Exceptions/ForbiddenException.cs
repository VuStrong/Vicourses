namespace CourseService.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, string details) : base(message)
        {
            Details = details;
        }

        public string? Details { get; }
    }
}
