namespace CourseService.Application.Dtos.Course
{
    public class CoursePublicCurriculumDto
    {
        public List<SectionInPublicCurriculumDto> Sections { get; set; } = [];
    }

    public class SectionInPublicCurriculumDto
    {
        public string Id { get; protected set; } = string.Empty;
        public string CourseId { get; protected set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public List<LessionInPublicCurriculumDto> Lessions { get; set; } = [];
    }

    public class LessionInPublicCurriculumDto
    {
        public string Id { get; protected set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string SectionId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
