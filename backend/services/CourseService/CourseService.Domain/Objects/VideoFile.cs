namespace CourseService.Domain.Objects
{
    public class VideoFile
    {
        public string FileId { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string? StreamFileUrl { get; set; }
        public int Length { get; set; }
    }
}
