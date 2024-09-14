using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models
{
    public record BannerRequest
    {
        [Required(ErrorMessage = "The bannerFile.fileId field is required")]
        public string FileId { get; set; } = null!;

        [Required(ErrorMessage = "The bannerFile.url field is required")]
        public string Url { get; set; } = null!;
    }

    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(40, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public BannerRequest? BannerFile { get; set; }
    }
}
