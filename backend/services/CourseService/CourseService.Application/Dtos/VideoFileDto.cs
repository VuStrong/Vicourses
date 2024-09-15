namespace CourseService.Application.Dtos
{
    public record VideoFileDto
    {
        public string FileId { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string FileName { get; set; } = null!;
    }
}
