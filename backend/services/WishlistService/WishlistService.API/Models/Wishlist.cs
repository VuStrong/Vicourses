using WishlistService.API.Application.Exceptions;

namespace WishlistService.API.Models
{
    public class Wishlist
    {
        private const int MaxItemsInWishlist = 50;
        private List<Course> _courses = [];
        private List<string> _enrolledCourseIds = [];

        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Email { get; private set; }
        public int Count { get; private set; }
        public IReadOnlyList<Course> Courses => _courses.AsReadOnly();
        public IReadOnlyList<string> EnrolledCourseIds => _enrolledCourseIds.AsReadOnly();

        public Wishlist(string userId, string email)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Email = email;
        }

        public void AddCourse(Course course)
        {
            if (_courses.Any(c => c.Id == course.Id))
            {
                return;
            }

            if (course.Status != CourseStatus.Published)
            {
                throw new ForbiddenException("Cannot add unpublished course to wishlist");
            }

            if (Count >= MaxItemsInWishlist)
            {
                throw new ForbiddenException($"Exceeded maximum number of courses in wishlist ({MaxItemsInWishlist})");
            }

            if (course.User.Id == UserId)
            {
                throw new ForbiddenException($"Cannot add owned course to wishlist");
            }

            if (_enrolledCourseIds.Any(id => id == course.Id))
            {
                throw new ForbiddenException($"Cannot add enrolled course to wishlist");
            }

            _courses.Insert(0, course);
            Count++;
        }

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
    }
}
