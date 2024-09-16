using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(40, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public string? ParentId { get; set; }
    }
}
