using CourseService.Application.Dtos;
using CourseService.Application.Dtos.Course;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Models.Course
{
    public class UpdateCourseRequest
    {
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string? Title { get; set; }

        public string? CategoryId { get; set; }
        public string? SubCategoryId { get; set; }

        [MinLength(100, ErrorMessage = "{0} must have minimum length of 100 words")]
        public string? Description { get; set; }

        public List<string>? Tags { get; set; }
        public List<string>? Requirements { get; set; }
        public List<string>? TargetStudents { get; set; }
        public List<string>? LearnedContents { get; set; }

        [Range(0, 100)]
        public decimal? Price { get; set; }
        public string? Language { get; set; }

        public ThumbnailRequest? Thumbnail { get; set; }
        public PreviewVideoRequest? PreviewVideo { get; set; }

        public UpdateCourseDto ToUpdateCourseDto()
        {
            ImageFileDto? thumbnail = Thumbnail != null ?
                new ImageFileDto(Thumbnail.FileId, Thumbnail.Url) : null;
            VideoFileDto? previewVideo = PreviewVideo != null ?
                new VideoFileDto
                {
                    FileId = PreviewVideo.FileId,
                    Url = PreviewVideo.Url,
                    FileName = PreviewVideo.FileName,
                } : null;

            return new UpdateCourseDto
            {
                Title = Title,
                CategoryId = CategoryId,
                SubCategoryId = SubCategoryId,
                Description = Description,
                Tags = Tags,
                Requirements = Requirements,
                TargetStudents = TargetStudents,
                LearnedContents = LearnedContents,
                Price = Price,
                Language = Language,
                Thumbnail = thumbnail,
                PreviewVideo = previewVideo
            };
        }
    }

    public record ThumbnailRequest
    {
        [Required(ErrorMessage = "The thumbnail.fileId field is required")]
        public string FileId { get; set; } = null!;

        [Required(ErrorMessage = "The thumbnail.url field is required")]
        public string Url { get; set; } = null!;
    }

    public record PreviewVideoRequest
    {
        [Required(ErrorMessage = "The thumbnail.fileId field is required")]
        public string FileId { get; set; } = null!;

        [Required(ErrorMessage = "The thumbnail.url field is required")]
        public string Url { get; set; } = null!;

        [Required(ErrorMessage = "The thumbnail.fileName field is required")]
        public string FileName { get; set; } = null!;
    }
}
