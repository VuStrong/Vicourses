namespace CourseService.Application.Dtos
{
    public class VideoFileDto
    {
        public string FileId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string? StreamFileUrl { get; set; }
        public int Length { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
