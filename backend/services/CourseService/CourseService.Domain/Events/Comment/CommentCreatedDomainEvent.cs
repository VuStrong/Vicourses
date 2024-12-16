namespace CourseService.Domain.Events.Comment
{
    public class CommentCreatedDomainEvent : DomainEvent
    {
        public Models.Comment Comment { get; set; }

        public CommentCreatedDomainEvent(Models.Comment comment)
        {
            Comment = comment;
        }
    }
}