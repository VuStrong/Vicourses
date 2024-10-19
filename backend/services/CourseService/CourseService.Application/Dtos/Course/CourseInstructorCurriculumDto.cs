namespace CourseService.Application.Dtos.Course
{
    public class CourseInstructorCurriculumDto
    {
        public List<SectionInInstructorCurriculumDto> Sections { get; set; } = [];
    }

    public class SectionInInstructorCurriculumDto
    {
        public string Id { get; protected set; } = string.Empty;
        public string CourseId { get; protected set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public int Duration { get; set; }
        public int LessonCount { get; set; }
        public List<LessonInInstructorCurriculumDto> Lessons { get; set; } = [];
    }

    public class LessonInInstructorCurriculumDto
    {
        public string Id { get; protected set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string SectionId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Duration { get; set; }
        public int Order { get; set; }
        public string Type { get; set; } = string.Empty;
        public VideoFileDto? Video {  get; set; }
    }
}
