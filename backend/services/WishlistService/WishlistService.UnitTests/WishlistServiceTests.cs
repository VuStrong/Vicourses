using AutoMapper;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using WishlistService.API.Application.Dtos;
using WishlistService.API.Application.Exceptions;
using WishlistService.API.Infrastructure.Repositories;
using WishlistService.API.Models;

namespace WishlistService.UnitTests
{
    public class WishlistServiceTests
    {
        private readonly ICourseRepository _courseRepositoryMock;
        private readonly IWishlistRepository _wishlistRepositoryMock;
        private readonly IMapper _mapperMock;

        public WishlistServiceTests()
        {
            _courseRepositoryMock = Substitute.For<ICourseRepository>();
            _wishlistRepositoryMock = Substitute.For<IWishlistRepository>();
            _mapperMock = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task AddCourseToUserWishlistAsync_ShouldThrow_WhenCourseNotFound()
        {
            var dto = new AddToWishlistDto
            {
                UserId = "a",
                CourseId = "b",
                Email = "c",
            };
            _wishlistRepositoryMock.FindByUserIdAsync(dto.UserId).ReturnsNull();
            _courseRepositoryMock.FindByIdAsync(dto.CourseId).ReturnsNull();

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);

            await Assert.ThrowsAsync<NotFoundException>(() => wishlistService.AddCourseToUserWishlistAsync(dto));
        }

        [Fact]
        public async Task AddCourseToUserWishlistAsync_ShouldThrow_WhenExceededMaximumNumberOfCoursesInWishlist()
        {
            var dto = new AddToWishlistDto
            {
                UserId = "a",
                CourseId = "b",
                Email = "c",
            };
            var wishlist = CreateWishlist(dto.UserId, dto.Email, 51);
            var course = new Course("id", "a", "a", new UserInCourse("id", "name", null));

            _wishlistRepositoryMock.FindByUserIdAsync(dto.UserId).Returns(wishlist);
            _courseRepositoryMock.FindByIdAsync(dto.CourseId).Returns(course);

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);

            await Assert.ThrowsAsync<ForbiddenException>(() => wishlistService.AddCourseToUserWishlistAsync(dto));
        }

        [Fact]
        public async Task AddCourseToUserWishlistAsync_ShouldThrow_WhenAddEnrolledCourse()
        {
            var dto = new AddToWishlistDto
            {
                UserId = "a",
                CourseId = "b",
                Email = "c",
            };
            var course = new Course("id", "a", "a", new UserInCourse("id", "name", null));
            var wishlist = CreateWishlist(dto.UserId, dto.Email);
            wishlist.EnrollCourse(course.Id);

            _wishlistRepositoryMock.FindByUserIdAsync(dto.UserId).Returns(wishlist);
            _courseRepositoryMock.FindByIdAsync(dto.CourseId).Returns(course);

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);

            await Assert.ThrowsAsync<ForbiddenException>(() => wishlistService.AddCourseToUserWishlistAsync(dto));
        }

        [Fact]
        public async Task AddCourseToUserWishlistAsync_ShouldCreateNewWishlist_WhenWishlistNotExists()
        {
            var dto = new AddToWishlistDto
            {
                UserId = "a",
                CourseId = "b",
                Email = "c",
            };
            var course = new Course("id", "a", "a", new UserInCourse("id", "name", null));

            _wishlistRepositoryMock.FindByUserIdAsync(dto.UserId).ReturnsNull();
            _courseRepositoryMock.FindByIdAsync(dto.CourseId).Returns(course);

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);
            
            // Act
            await wishlistService.AddCourseToUserWishlistAsync(dto);

            // Assert
            await _wishlistRepositoryMock.ReceivedWithAnyArgs(1).InsertWishlistAsync(default!);
        }

        [Fact]
        public async Task AddCourseToUserWishlistAsync_ShouldAddCourse()
        {
            var dto = new AddToWishlistDto
            {
                UserId = "a",
                CourseId = "b",
                Email = "c",
            };
            var course = new Course(dto.CourseId, "a", "a", new UserInCourse("id", "name", null));
            var wishlist = CreateWishlist(dto.UserId, dto.Email);

            _wishlistRepositoryMock.FindByUserIdAsync(dto.UserId).Returns(wishlist);
            _courseRepositoryMock.FindByIdAsync(dto.CourseId).Returns(course);

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);

            // Act
            await wishlistService.AddCourseToUserWishlistAsync(dto);

            // Assert
            await _wishlistRepositoryMock.ReceivedWithAnyArgs(1).UpdateWishlistAsync(default!);
            Assert.Equal(1, wishlist.Count);
            Assert.Contains(wishlist.Courses, c => c.Id == course.Id);
        }

        [Fact]
        public async Task RemoveCourseFromUserWishlistAsync_ShouldThrow_WhenWishlistNotFound()
        {
            const string userId = "a";
            const string courseId = "b";
            _wishlistRepositoryMock.FindByUserIdAsync(userId).ReturnsNull();

            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);

            await Assert.ThrowsAsync<NotFoundException>(() => wishlistService.RemoveCourseFromUserWishlistAsync(userId, courseId));
        }

        [Fact]
        public async Task RemoveCourseFromUserWishlistAsync_ShouldRemoveCourse()
        {
            // Arrange
            const string userId = "a";
            const string courseId = "b";
            var wishlist = new Wishlist(userId, "email");
            var course = new Course(courseId, "a", "a", new UserInCourse("id", "name", null));

            wishlist.AddCourse(course);

            _wishlistRepositoryMock.FindByUserIdAsync(userId).Returns(wishlist);

            // Act
            var wishlistService = new API.Application.Services.WishlistService(_courseRepositoryMock, _wishlistRepositoryMock, _mapperMock);
            await wishlistService.RemoveCourseFromUserWishlistAsync(userId, courseId);

            // Assert
            await _wishlistRepositoryMock.ReceivedWithAnyArgs(1).UpdateWishlistAsync(default!);
            Assert.Equal(0, wishlist.Count);
            Assert.Empty(wishlist.Courses);
        }

        private Wishlist CreateWishlist(string userId, string email, int? count = null)
        {
            var wishlist = new Wishlist(userId, email);

            if (count != null)
            {
                typeof(Wishlist).GetProperty("Count")!.SetValue(wishlist, count.Value, null);
            }

            return wishlist;
        }
    }
}