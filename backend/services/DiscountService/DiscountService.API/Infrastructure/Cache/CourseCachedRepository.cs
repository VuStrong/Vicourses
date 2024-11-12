using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DiscountService.API.Infrastructure.Cache
{
    public class CourseCachedRepository : ICourseCachedRepository
    {
        private readonly DiscountServiceDbContext _dbContext;
        private readonly IDatabase _redis;

        public CourseCachedRepository(DiscountServiceDbContext dbContext, IConnectionMultiplexer muxer)
        {
            _dbContext = dbContext;
            _redis = muxer.GetDatabase();
        }

        public async Task<decimal?> GetCoursePriceAsync(string courseId)
        {
            var key = $"course-price:{courseId}";
            var price = await _redis.StringGetAsync(key);

            if (string.IsNullOrEmpty(price))
            {
                var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

                if (course == null)
                {
                    return null;
                }

                await _redis.StringSetAsync(key, course.Price.ToString(), TimeSpan.FromHours(1));

                return course.Price;
            }

            return decimal.Parse(price!);
        }

        public async Task DeleteCoursePriceCachedAsync(string courseId)
        {
            var key = $"course-price:{courseId}";
            await _redis.KeyDeleteAsync(key);
        }
    }
}
