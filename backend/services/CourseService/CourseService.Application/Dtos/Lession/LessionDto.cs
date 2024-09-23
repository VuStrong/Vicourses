namespace CourseService.Application.Dtos.Lession
{
    public class LessionDto
    {
        public string Id { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string SectionId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        public VideoFileDto? Video { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
