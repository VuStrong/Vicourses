using CourseService.Application.Dtos.Course;
using CourseService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseService.API.Requests.Course
{
    public class UpdateCourseRequest
    {
        [StringLength(60, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        public string? Title { get; set; }

        public string? CategoryId { get; set; }
        public string? SubCategoryId { get; set; }

        [MinLength(100, ErrorMessage = "{0} must have minimum length of 100 words")]
        public string? Description { get; set; }

        [Length(0, 10)]
        public List<string>? Tags { get; set; }

        [Length(0, 10)]
        public List<string>? Requirements { get; set; }

        [Length(0, 10)]
        public List<string>? TargetStudents { get; set; }

        [Length(0, 10)]
        public List<string>? LearnedContents { get; set; }

        /// <example>19.99</example>>
        public decimal? Price { get; set; }
        public string? Locale { get; set; }

        [EnumDataType(typeof(CourseLevel))]
        public CourseLevel? Level { get; set; }

        [EnumDataType(typeof(CourseStatus))]
        public CourseStatus? Status { get; set; }

        public string? ThumbnailToken { get; set; }
        public string? PreviewVideoToken { get; set; }

        public UpdateCourseDto ToUpdateCourseDto()
        {
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
                Locale = Locale,
                Level = Level,
                Status = Status,
                ThumbnailToken = ThumbnailToken,
                PreviewVideoToken = PreviewVideoToken,
            };
        }
    }
}
