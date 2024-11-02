namespace CourseService.Application.Exceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(string id) : base("comment", id) { }
    }
}
