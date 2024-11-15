using AutoMapper;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.Exceptions;
using WishlistService.API.Infrastructure.Repositories;
using WishlistService.API.Models;

namespace WishlistService.API.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private const int MaxItemsInWishlist = 50;

        private readonly ICourseRepository _courseRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public WishlistService(
            ICourseRepository courseRepository,
            IWishlistRepository wishlistRepository,
            IMapper mapper)
        {
            _courseRepository = courseRepository;
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        public async Task<WishlistDto> GetUserWishlistAsync(string userId, CancellationToken cancellationToken = default)
        {
            var wishlist = await _wishlistRepository.FindByUserIdAsync(userId, cancellationToken);

            if (wishlist == null)
            {
                throw new NotFoundException($"Wishlist of user {userId} was not found");
            }

            return _mapper.Map<WishlistDto>(wishlist);
        }

        public async Task<WishlistDto> AddCourseToUserWishlistAsync(AddToWishlistDto data)
        {
            var wishlist = await _wishlistRepository.FindByUserIdAsync(data.UserId);
            var existsWishlist = true;

            if (wishlist == null)
            {
                existsWishlist = false;
                wishlist = new Wishlist(data.UserId, data.Email);
            }

            var course = await _courseRepository.FindByIdAsync(data.CourseId);

            if (course == null)
            {
                throw new NotFoundException("course", data.CourseId);
            }

            ValidateAddCourseToWishlist(wishlist, course);

            wishlist.AddCourse(course);

            if (existsWishlist)
            {
                await _wishlistRepository.UpdateWishlistAsync(wishlist);
            }
            else
            {
                await _wishlistRepository.InsertWishlistAsync(wishlist);
            }

            return _mapper.Map<WishlistDto>(wishlist);
        }

        public async Task<WishlistDto> RemoveCourseFromUserWishlistAsync(string userId, string courseId)
        {
            var wishlist = await _wishlistRepository.FindByUserIdAsync(userId);

            if (wishlist == null)
            {
                throw new NotFoundException($"Wishlist of user {userId} was not found");
            }

            wishlist.RemoveCourse(courseId);

            await _wishlistRepository.UpdateWishlistAsync(wishlist);

            return _mapper.Map<WishlistDto>(wishlist);
        }

        private static void ValidateAddCourseToWishlist(Wishlist wishlist, Course course)
        {
            if (course.Status != CourseStatus.Published)
            {
                throw new ForbiddenException("Cannot add unpublished course to wishlist");
            }
            if (wishlist.Count >= MaxItemsInWishlist)
            {
                throw new ForbiddenException($"Exceeded maximum number of courses in wishlist ({MaxItemsInWishlist})");
            }
            if (course.User.Id == wishlist.UserId)
            {
                throw new ForbiddenException($"Cannot add owned course to wishlist");
            }
            if (wishlist.EnrolledCourseIds.Any(id => id == course.Id))
            {
                throw new ForbiddenException($"Cannot add enrolled course to wishlist");
            }
        }
    }
}
