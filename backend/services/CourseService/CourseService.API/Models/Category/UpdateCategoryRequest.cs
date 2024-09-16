using CourseService.Application.Dtos.Category;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Category
{
    public class UpdateCategoryRequest
    {
        [StringLength(40, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? Name { get; set; }

        public string? ParentId { get; set; }

        public bool? SetRoot { get; set; }

        public UpdateCategoryDto ToUpdateCategoryDto()
        {
            return new UpdateCategoryDto
            {
                Name = Name,
                ParentId = ParentId,
                SetRoot = SetRoot ?? false
            };
        }
    }
}
