using WishlistService.API.Infrastructure.Repositories;
using WishlistService.API.Models;

namespace WishlistService.API.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IWishlistRepository _wishlistRepository;

        public CourseService(ICourseRepository courseRepository, IWishlistRepository wishlistRepository)
        {
            _courseRepository = courseRepository;
            _wishlistRepository = wishlistRepository;
        }

        public async Task AddOrUpdateCourseAsync(Course course)
        {
            var courseInDb = await _courseRepository.FindByIdAsync(course.Id);

            if (courseInDb != null)
            {
                await UpdateCourseAsync(courseInDb, course);
            }
            else
            {
                await _courseRepository.InsertCourseAsync(course);
            }
        }

        public async Task UnpublishCourseAsync(string courseId)
        {
            await _courseRepository.UpdateStatusAsync(courseId, CourseStatus.Unpublished);

            await _wishlistRepository.RemoveCourseInWishlistsAsync(courseId);
        }

        private async Task UpdateCourseAsync(Course courseInDb, Course targetCourse)
        {
            var statusUpdated = false;
            var infoUpdated = false;

            if (courseInDb.Title != targetCourse.Title)
            {
                courseInDb.Title = targetCourse.Title;
                courseInDb.TitleCleaned = targetCourse.TitleCleaned;
                infoUpdated = true;
            }
            if (courseInDb.Price != targetCourse.Price)
            {
                courseInDb.Price = targetCourse.Price;
                courseInDb.IsPaid = targetCourse.IsPaid;
                infoUpdated = true;
            }
            if (courseInDb.Rating != targetCourse.Rating)
            {
                courseInDb.Rating = targetCourse.Rating;
                infoUpdated = true;
            }
            if (courseInDb.ThumbnailUrl != targetCourse.ThumbnailUrl)
            {
                courseInDb.ThumbnailUrl = targetCourse.ThumbnailUrl;
                infoUpdated = true;
            }
            if (courseInDb.Status != targetCourse.Status)
            {
                courseInDb.Status = targetCourse.Status;
                statusUpdated = true;
                infoUpdated = true;
            }

            if (infoUpdated)
            {
                await _courseRepository.UpdateCourseAsync(courseInDb);
            }

            if (!statusUpdated && infoUpdated && courseInDb.Status == CourseStatus.Published)
            {
                await _wishlistRepository.UpdateCourseInWishlistsAsync(courseInDb);
            }

            if (statusUpdated && courseInDb.Status == CourseStatus.Unpublished)
            {
                await _wishlistRepository.RemoveCourseInWishlistsAsync(courseInDb.Id);
            }
        }
    }
}
