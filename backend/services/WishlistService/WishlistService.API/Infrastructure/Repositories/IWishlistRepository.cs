using WishlistService.API.Models;

namespace WishlistService.API.Infrastructure.Repositories
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> CheckCourseInWishlistAsync(string userId, string courseId, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetCourseIdsInWishlistAsync(string userId, CancellationToken cancellationToken = default);
        Task InsertWishlistAsync(Wishlist wishlist);
        Task UpdateWishlistAsync(Wishlist wishlist);

        /// <summary>
        /// Find all wishlists containing the course and update it
        /// </summary>
        /// <param name="course">Course to update</param>
        Task UpdateCourseInWishlistsAsync(Course course);

        /// <summary>
        /// Find all wishlists containing the course and remove it
        /// </summary>
        /// <param name="courseId">Id to find course in wishlist</param>
        Task RemoveCourseInWishlistsAsync(string courseId);
    }
}
