using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Course
{
    public class CreateCourseRequest
    {
        [Required]
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required]
        public string CategoryId { get; set; } = null!;
        [Required]
        public string SubCategoryId { get; set; } = null!;

        public string? Description { get; set; }
    }
}
