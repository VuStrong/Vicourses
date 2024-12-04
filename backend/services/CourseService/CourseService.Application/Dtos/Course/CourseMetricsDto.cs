namespace CourseService.Application.Dtos.Course
{
    public class CourseMetricsDto
    {
        public int SectionsCount { get; set; }
        public int LessonsCount { get; set; }
        public int QuizLessonsCount { get; set; }
        public int TotalVideoDuration { get; set; }
    }
}
