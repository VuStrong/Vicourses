namespace CourseService.Application.Dtos
{
    public class VideoFileDto
    {
        public string OriginalFileName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
