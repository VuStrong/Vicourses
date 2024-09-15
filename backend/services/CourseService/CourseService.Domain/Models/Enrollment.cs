using CourseService.Domain.Constracts;

namespace CourseService.Domain.Models
{
    public class Enrollment : IBaseEntity
    {
        public string Id { get; protected set; } = null!;
        public string CourseId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime Time { get; set; }

        public static Enrollment Create(string courseId, string userId)
        {
            return new Enrollment
            {
                CourseId = courseId,
                UserId = userId,
                Time = DateTime.Now,
            };
        }
    }
}
