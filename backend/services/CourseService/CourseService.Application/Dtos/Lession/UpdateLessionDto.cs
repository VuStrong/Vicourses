namespace CourseService.Application.Dtos.Lession
{
    public class UpdateLessionDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public UpdateVideoFileDto? Video { get; set; }
    }
}
