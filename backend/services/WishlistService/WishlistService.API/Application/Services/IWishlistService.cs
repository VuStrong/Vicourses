using WishlistService.API.Application.Dtos;

namespace WishlistService.API.Application.Services
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetUserWishlistAsync(string userId, CancellationToken cancellationToken = default);

        Task<WishlistDto> AddCourseToUserWishlistAsync(AddToWishlistDto data);
        Task<WishlistDto> RemoveCourseFromUserWishlistAsync(string userId, string courseId);
    }
}
