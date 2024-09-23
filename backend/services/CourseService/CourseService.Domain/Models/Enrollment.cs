using CourseService.Domain.Contracts;

namespace CourseService.Domain.Models
{
    public class Enrollment : IBaseEntity
    {
        public string Id { get; protected set; }
        public string CourseId { get; set; }
        public string UserId { get; set; }
        public DateTime Time { get; set; }

        private Enrollment(string id, string courseId, string userId)
        {
            Id = id;
            CourseId = courseId;
            UserId = userId;
        }

        public static Enrollment Create(string courseId, string userId)
        {
            var id = Guid.NewGuid().ToString();

            return new Enrollment(id, courseId, userId)
            {
                Time = DateTime.Now,
            };
        }
    }
}
