namespace CourseService.Application.Dtos
{
    public class VideoFileDto
    {
        public string FileId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? StreamFileUrl { get; set; }
        public int Length { get; set; }
    }
}
