namespace CourseService.Domain.Objects
{
    public class CourseMetrics
    {
        public int SectionsCount { get; internal set; }
        public int LessonsCount { get; internal set; }
        public int QuizLessonsCount { get; internal set; }
        public int TotalVideoDuration { get; internal set; }
    }
}
