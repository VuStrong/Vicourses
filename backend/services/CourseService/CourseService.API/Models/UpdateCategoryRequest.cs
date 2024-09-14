using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models
{
    public class UpdateCategoryRequest
    {
        [StringLength(40, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? Name { get; set; }

        public BannerRequest? BannerFile { get; set; }
    }
}
