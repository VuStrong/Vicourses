namespace CourseService.Application.Dtos
{
    public class UploadFileDto
    {
        public string FileId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
    }
}