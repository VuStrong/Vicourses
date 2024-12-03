namespace WishlistService.API.Models
{
    public class Wishlist
    {
        private List<Course> _courses = [];
        private List<string> _enrolledCourseIds = [];

        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Email { get; set; }
        public int Count { get; private set; }
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();
        public IReadOnlyList<string> EnrolledCourseIds => _enrolledCourseIds.AsReadOnly();

        public Wishlist(string userId, string email)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Email = email;
        }

        /// <summary>
        /// Add a course to wishlist, if the course already exists in wishlist then do nothing
        /// </summary>
        public void AddCourse(Course course)
        {
            if (_courses.Any(c => c.Id == course.Id))
            {
                return;
            }

            _courses.Insert(0, course);
            Count++;
        }

        /// <summary>
        /// Remove one course by Id from wishlist
        /// </summary>
        public void RemoveCourse(string courseId)
        {
            int count = _courses.RemoveAll(c => c.Id == courseId);
            Count -= count;
        }

        public void EnrollCourse(string courseId)
        {
            if (!_enrolledCourseIds.Any(id => id == courseId))
            {
                _enrolledCourseIds.Add(courseId);
            }
        }

        public void RemoveEnrollCourse(string courseId)
        {
            _enrolledCourseIds.RemoveAll(id => id == courseId);
        }
    }
}
