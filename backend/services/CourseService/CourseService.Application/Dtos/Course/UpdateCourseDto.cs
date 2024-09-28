using CourseService.Domain.Enums;

namespace CourseService.Application.Dtos.Course
{
    public class UpdateCourseDto
    {
        public string? Title { get; set; }
        public string? CategoryId { get; set; }
        public string? SubCategoryId { get; set; }
        public string? Description { get; set; }
        public List<string>? Tags { get; set; }
        public List<string>? Requirements { get; set; }
        public List<string>? TargetStudents { get; set; }
        public List<string>? LearnedContents { get; set; }
        public decimal? Price { get; set; }
        public string? Language {  get; set; }
        public CourseLevel? Level { get; set; }
        public CourseStatus? Status { get; set; }
        public UpdateImageFileDto? Thumbnail { get; set; }
        public UpdateVideoFileDto? PreviewVideo { get; set; }
    }
}
