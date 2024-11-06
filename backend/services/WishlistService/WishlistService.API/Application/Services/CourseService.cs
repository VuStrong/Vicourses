using MongoDB.Driver;
using WishlistService.API.Models;

namespace WishlistService.API.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Wishlist> _wishlistCollection;

        public CourseService(IMongoCollection<Course> courseCollection, IMongoCollection<Wishlist> wishlistCollection)
        {
            _courseCollection = courseCollection;
            _wishlistCollection = wishlistCollection;
        }

        public async Task AddOrUpdateCourseAsync(Course course)
        {
            var courseInDb = await _courseCollection
                .Find(Builders<Course>.Filter.Eq(c => c.Id, course.Id))
                .FirstOrDefaultAsync();

            if (courseInDb != null)
            {
                await UpdateCourseAsync(courseInDb, course);
            }
            else
            {
                await _courseCollection.InsertOneAsync(course);
            }
        }

        public async Task UnpublishCourseAsync(string courseId)
        {
            await _courseCollection.UpdateOneAsync(
                Builders<Course>.Filter.Eq(c => c.Id, courseId),
                Builders<Course>.Update.Set(c => c.Status, CourseStatus.Unpublished)
            );

            await RemoveCourseInWishlistsAsync(courseId);
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
                await _courseCollection.ReplaceOneAsync(
                    Builders<Course>.Filter.Eq(c => c.Id, courseInDb.Id),
                    courseInDb
                );
            }

            if (!statusUpdated && infoUpdated && courseInDb.Status == CourseStatus.Published)
            {
                await UpdateCourseInWishlistsAsync(courseInDb);
            }

            if (statusUpdated && courseInDb.Status == CourseStatus.Unpublished)
            {
                await RemoveCourseInWishlistsAsync(courseInDb.Id);
            }
        }

        private async Task UpdateCourseInWishlistsAsync(Course course)
        {
            var filter = Builders<Wishlist>.Filter.Eq("Courses._id", course.Id);
            var update = Builders<Wishlist>.Update.Set("Courses.$", course);

            await _wishlistCollection.UpdateManyAsync(filter, update);
        }

        private async Task RemoveCourseInWishlistsAsync(string courseId)
        {
            var filter = Builders<Wishlist>.Filter.Eq("Courses._id", courseId);
            var update = Builders<Wishlist>.Update
                .Inc(x => x.Count, -1)
                .PullFilter("Courses", Builders<Course>.Filter.Eq("_id", courseId));

            await _wishlistCollection.UpdateManyAsync(filter, update);
        }
    }
}
