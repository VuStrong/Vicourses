namespace CourseService.Application.Dtos.Course
{
    public class CourseDetailDto : CourseDto
    {
        public string? Description { get; set; }
        public string[] Requirements { get; set; } = [];
        public string[] TargetStudents { get; set; } = [];
        public VideoFileDto? PreviewVideo { get; set; }
    }
}
