using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Comment
{
    public class CreateCommentRequest
    {
        [Required]
        public string Content { get; set; } = null!;


        public string? ReplyToId { get; set; }
    }
}
