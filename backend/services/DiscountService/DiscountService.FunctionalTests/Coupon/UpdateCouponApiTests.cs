using DiscountService.API.Infrastructure;
using DiscountService.API.Models;
using DiscountService.API.Requests;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.FunctionalTests.Coupon
{
    [Collection("DiscountAPI Test collection")]
    public class UpdateCouponApiTests : IAsyncLifetime
    {
        private readonly DiscountApiTestFactory _factory;
        private readonly HttpClient _httpClient;

        public UpdateCouponApiTests(DiscountApiTestFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Should_ReturnNotfound_WhenCouponNotExists()
        {
            // Arrange
            var request = new SetCouponActiveRequest
            {
                IsActive = true,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "userId")
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            const string couponCode = "abc";

            // Act
            var response = await _httpClient.PatchAsJsonAsync($"/api/ds/v1/coupons/{couponCode}", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotCreatorOfTheCoupon()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";
            const string couponCode = "abc";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));
            await PrepareCoupons(API.Models.Coupon.Create(couponCode, courseInstructorId, courseId, 1, 1, 50));

            var request = new SetCouponActiveRequest
            {
                IsActive = true,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "anotherUserId")
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PatchAsJsonAsync($"/api/ds/v1/coupons/{couponCode}", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnOK_WhenValid()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";
            const string couponCode = "abc";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));
            await PrepareCoupons(API.Models.Coupon.Create(couponCode, courseInstructorId, courseId, 1, 1, 50));

            var request = new SetCouponActiveRequest
            {
                IsActive = false,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", courseInstructorId)
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PatchAsJsonAsync($"/api/ds/v1/coupons/{couponCode}", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var couponInDb = await GetCoupon(couponCode);
            Assert.NotNull(couponInDb);
            Assert.False(couponInDb.IsActive);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        private async Task<API.Models.Coupon?> GetCoupon(string code)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();
            return await dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == code);
        }

        private async Task PrepareCourses(params Course[] courses)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();
            dbContext.Courses.AddRange(courses);
            await dbContext.SaveChangesAsync();
        }

        private async Task PrepareCoupons(params API.Models.Coupon[] coupons)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();
            dbContext.Coupons.AddRange(coupons);
            await dbContext.SaveChangesAsync();
        }
    }
}
