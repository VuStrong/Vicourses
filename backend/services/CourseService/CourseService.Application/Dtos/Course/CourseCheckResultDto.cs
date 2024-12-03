namespace CourseService.Application.Dtos.Course
{
    public class CourseCheckResultDto
    {
        public bool IsValid { get; set; }
        public List<string> MissingRequirements { get; set; } = [];
    }
}
