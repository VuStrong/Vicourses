namespace CourseService.Domain.Models
{
    public class SectionWithLessions : Section
    {
        public List<Lession> Lessions { get; set; } = [];

        private SectionWithLessions(string id, string title, string courseId) : base(id, title, courseId) { }
    }
}
