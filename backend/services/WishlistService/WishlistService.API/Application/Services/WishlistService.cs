using AutoMapper;
using MongoDB.Driver;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.Exceptions;
using WishlistService.API.Models;

namespace WishlistService.API.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Wishlist> _wishlistCollection;
        private readonly IMapper _mapper;

        public WishlistService(
            IMongoCollection<Course> courseCollection, 
            IMongoCollection<Wishlist> wishlistCollection,
            IMapper mapper)
        {
            _courseCollection = courseCollection;
            _wishlistCollection = wishlistCollection;
            _mapper = mapper;
        }

        public async Task<WishlistDto> GetUserWishlistAsync(string userId, CancellationToken cancellationToken = default)
        {
            var wishlist = await _wishlistCollection
                .Find(Builders<Wishlist>.Filter.Eq(x => x.UserId, userId))
                .FirstOrDefaultAsync(cancellationToken);

            if (wishlist == null)
            {
                throw new NotFoundException($"Wishlist of user {userId} was not found");
            }

            return _mapper.Map<WishlistDto>(wishlist);
        }

        public async Task<WishlistDto> AddCourseToUserWishlistAsync(AddToWishlistDto data)
        {
            var wishlist = await _wishlistCollection
                .Find(Builders<Wishlist>.Filter.Eq(x => x.UserId, data.UserId))
                .FirstOrDefaultAsync();
            var existsWishlist = true;

            if (wishlist == null)
            {
                existsWishlist = false;
                wishlist = new Wishlist(data.UserId, data.Email);
            }

            var course = await _courseCollection
                .Find(Builders<Course>.Filter.Eq(x => x.Id, data.CourseId))
                .FirstOrDefaultAsync();

            if (course == null)
            {
                throw new NotFoundException("course", data.CourseId);
            }

            wishlist.AddCourse(course);

            if (existsWishlist)
            {
                await _wishlistCollection.ReplaceOneAsync(
                    Builders<Wishlist>.Filter.Eq(x => x.Id, wishlist.Id),
                    wishlist
                );
            }
            else
            {
                await _wishlistCollection.InsertOneAsync(wishlist);
            }

            return _mapper.Map<WishlistDto>(wishlist);
        }

        public async Task<WishlistDto> RemoveCourseFromUserWishlistAsync(string userId, string courseId)
        {
            var wishlist = await _wishlistCollection
                .Find(Builders<Wishlist>.Filter.Eq(x => x.UserId, userId))
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                throw new NotFoundException($"Wishlist of user {userId} was not found");
            }

            wishlist.RemoveCourse(courseId);

            await _wishlistCollection.ReplaceOneAsync(
                Builders<Wishlist>.Filter.Eq(x => x.Id, wishlist.Id),
                wishlist
            );

            return _mapper.Map<WishlistDto>(wishlist);
        }
    }
}
