using DiscountService.API.Requests;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using DiscountService.API.Infrastructure;
using DiscountService.API.Models;
using Microsoft.Extensions.DependencyInjection;
using DiscountService.API.Application.Dtos.Coupon;
using System.Net;
using DiscountService.API.Application.Dtos;

namespace DiscountService.FunctionalTests.Coupon
{
    [Collection("DiscountAPI Test collection")]
    public class ConsumeCouponApiTests : IAsyncLifetime
    {
        private readonly DiscountApiTestFactory _factory;
        private readonly HttpClient _httpClient;

        public ConsumeCouponApiTests(DiscountApiTestFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Should_ReturnOKAndNotOK_WhenTwoRequestsConsumeSameCouponWhichRemainIsOne()
        {
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));

            var createdCoupon = await CreateCoupon(courseInstructorId, new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 1,
                Discount = 50,
            });

            string accessTokenOfRequest1 = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "userId1")
            });
            string accessTokenOfRequest2 = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "userId2")
            });

            var client1 = _factory.CreateClient();
            var client2 = _factory.CreateClient();

            client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenOfRequest1);
            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenOfRequest2);

            var responses = await Task.WhenAll(
                client1.PostAsync($"/api/ds/v1/coupons/{createdCoupon.Code}/consume?courseId={courseId}", null),
                client2.PostAsync($"/api/ds/v1/coupons/{createdCoupon.Code}/consume?courseId={courseId}", null)
            );

            Assert.Contains(responses, r => r.StatusCode == HttpStatusCode.OK);
            Assert.Contains(responses, r => r.StatusCode != HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_ReturnForbidden_WhenSameUserConsumeCouponTwice()
        {
            const string courseId = "course1";
            const string courseInstructorId = "instructor1";

            await PrepareCourses(new Course(courseId, courseInstructorId, 10));

            var createdCoupon = await CreateCoupon(courseInstructorId, new CreateCouponRequest
            {
                CourseId = courseId,
                Days = 1,
                Availability = 10,
                Discount = 50,
            });

            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", "userId1")
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync($"/api/ds/v1/coupons/{createdCoupon.Code}/consume?courseId={courseId}", null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await _httpClient.PostAsync($"/api/ds/v1/coupons/{createdCoupon.Code}/consume?courseId={courseId}", null);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
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

        private async Task<CouponDto> CreateCoupon(string userId, CreateCouponRequest request)
        {
            string accessToken = JwtHelper.GenerateJwtToken(new List<Claim>
            {
                new("sub", userId)
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsJsonAsync("/api/ds/v1/coupons", request);
            var result = await response.Content.ReadFromJsonAsync<CouponDto>();

            return result!;
        }
    }
}
