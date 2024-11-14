using DiscountService.API.Application.Dtos.Coupon;
using DiscountService.API.Infrastructure;
using DiscountService.API.Models;
using DiscountService.API.Requests;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DiscountService.FunctionalTests.Coupon
{
    [Collection("DiscountAPI Test collection")]
    public class CreateCouponApiTests : IAsyncLifetime
    {
        private readonly DiscountApiTestFactory _factory;
        private readonly HttpClient _httpClient;

        public CreateCouponApiTests(DiscountApiTestFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenUserIsNotInstructorOfTheCourse()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 0));

            var request = new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 1,
                Discount = 50,
            };
            
            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "anotherUserId")
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenCourseIsFree()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 0));

            var request = new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 1,
                Discount = 50,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", courseInstructorId)
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenCourseAlreadyHaveAnActiveCoupon()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));

            var request = new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 1,
                Discount = 50,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", courseInstructorId)
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Act
            response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_ReturnCreatedAndCourse_WhenValid()
        {
            // Arrange
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));

            var request = new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 1,
                Discount = 50,
            };

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", courseInstructorId)
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);
            var result = await response.Content.ReadFromJsonAsync<CouponDto>();

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEmpty(result!.Code);
            Assert.Equal(result.CourseId, request.CourseId);
            Assert.Equal(courseInstructorId, result.CreatorId);
            Assert.Equal(result.Count, request.Availability);
            Assert.Equal(result.Remain, request.Availability);
            Assert.Equal(result.ExpiryDate, DateTime.Today.AddDays(request.Days));
            Assert.Equal(result.Discount, request.Discount);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }

        private async Task PrepareCourses(params Course[] courses)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DiscountServiceDbContext>();
            dbContext.Courses.AddRange(courses);
            await dbContext.SaveChangesAsync();
        }
    }
}
