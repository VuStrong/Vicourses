namespace CourseService.Application.Dtos
{
    public record UpdateImageFileDto
    {
        public required string FileId { get; set; }
        public required string Url { get; set; }
    }
}
