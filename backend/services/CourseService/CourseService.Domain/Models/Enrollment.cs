namespace CourseService.Domain.Models
{
    public class Enrollment : Entity, IBaseEntity
    {
        public string Id { get; private set; }
        public string CourseId { get; private set; }
        public string UserId { get; private set; }
        public DateTime Time { get; private set; }

        private Enrollment(string id, string courseId, string userId)
        {
            Id = id;
            CourseId = courseId;
            UserId = userId;
        }

        internal static Enrollment Create(string courseId, string userId)
        {
            var id = Guid.NewGuid().ToString();

            return new Enrollment(id, courseId, userId)
            {
                Time = DateTime.Now,
            };
        }
    }
}
