namespace CourseService.Application.Dtos
{
    public record UpdateVideoFileDto
    {
        public required string FileId { get; set; }
        public required string Url { get; set; }
        public required string FileName { get; set; }
    }
}
