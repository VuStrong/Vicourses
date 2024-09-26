namespace CourseService.Domain.Models
{
    public class SectionWithLessions : Section
    {
        public List<Lession> Lessions { get; set; } = [];
        public int Duration { get; set; }
        public int LessionCount { get; set; }

        private SectionWithLessions(string id, string title, string courseId, string userId) : base(id, title, courseId, userId) { }
    }
}
