using DiscountService.API.Application.Dtos;
using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Infrastructure;
using DiscountService.API.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DiscountService.FunctionalTests.Coupon
{
    [Collection("DiscountAPI Test collection")]
    public class GetCouponsApiTests : IAsyncLifetime
    {
        private readonly DiscountApiTestFactory _factory;
        private readonly HttpClient _httpClient;

        public GetCouponsApiTests(DiscountApiTestFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();

            PrepareCoursesAndCoupons();
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotInstructorOfTheCourse()
        {
            // Arrange
            const string courseId = "course1";
            const string instructorOfOtherCourse = "instructor2";

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", instructorOfOtherCourse)
            });
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.GetAsync($"/api/ds/v1/coupons?courseId={courseId}");

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnOkAndCoupons_WhenUserIsInstructorOfTheCourse()
        {
            // Arrange
            const string courseId = "course1";
            const string instructorId = "instructor1";
            const int limit = 10;

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", instructorId)
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.GetAsync($"/api/ds/v1/coupons?courseId={courseId}&limit={limit}");
            var result = await response.Content.ReadFromJsonAsync<PagedResult<CouponDto>>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(limit, result!.Limit);
            Assert.Equal(instructorId, result.Items.First().CreatorId);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        private void PrepareCoursesAndCoupons()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();

            dbContext.Courses.AddRange(
                new Course("course1", "instructor1", 0),
                new Course("course2", "instructor2", 0)
            );

            dbContext.Coupons.AddRange(
                API.Models.Coupon.Create("code1", "instructor1", "course1", 0, 0, 0),
                API.Models.Coupon.Create("code2", "instructor1", "course1", 0, 0, 0),
                API.Models.Coupon.Create("code3", "instructor2", "course2", 0, 0, 0),
                API.Models.Coupon.Create("code4", "instructor2", "course2", 0, 0, 0)
            );

            dbContext.SaveChanges();
        }
    }
}
