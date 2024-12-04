using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StatisticsService.API.Application.Dtos;
using StatisticsService.API.Infrastructure;
using StatisticsService.API.Models;
using System.Text.Json;

namespace StatisticsService.API.Application.Services
{
    public class InstructorPerformanceStatistician : IInstructorPerformanceStatistician
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public InstructorPerformanceStatistician(StatisticsServiceDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<InstructorPerformanceDto> GetInstructorPerformanceAsync(string instructorId, DateScope dateScope = DateScope.Month,
            string? courseId = null, CancellationToken cancellationToken = default)
        {
            var cachedKey = $"instructor-performance:{instructorId}:course:{courseId}:scope:{dateScope}";
            var cachedData = await _cache.GetStringAsync(cachedKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<InstructorPerformanceDto>(cachedData)!;
            }

            var query = _dbContext.InstructorMetrics.Where(m => m.InstructorId == instructorId);

            if (!string.IsNullOrEmpty(courseId))
            {
                query = query.Where(m => m.CourseId == courseId);
            }

            var metricsQuery = BuildMetricsQuery(query, dateScope);

            var metrics = await metricsQuery.ToListAsync(cancellationToken);

            var data = new InstructorPerformanceDto()
            {
                Scope = dateScope,
                Metrics = metrics,
                TotalEnrollmentCount = metrics.Sum(m => m.EnrollmentCount),
                TotalRevenue = metrics.Sum(m => m.Revenue),
                TotalRefundCount = metrics.Sum(m => m.RefundCount),
            };

            await _cache.SetStringAsync(cachedKey, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return data;
        }

        private static IQueryable<InstructorMetricsDto> BuildMetricsQuery(IQueryable<InstructorMetric> query, DateScope dateScope)
        {
            DateOnly to = DateOnly.FromDateTime(DateTime.Today);
            DateOnly? from = null;

            if (dateScope == DateScope.Week)
            {
                from = to.AddDays(-7);

                return query.Where(m => m.Date >= from && m.Date <= to)
                    .GroupBy(m => m.Date)
                    .Select(m => new InstructorMetricsDto
                    {
                        Label = m.Key.ToString(),
                        EnrollmentCount = m.Sum(x => x.EnrollmentCount),
                        Revenue = m.Sum(x => x.Revenue),
                        RefundCount = m.Sum(x => x.RefundCount),
                    });
            }
            else if (dateScope == DateScope.Month)
            {
                from = to.AddMonths(-1);

                return query.Where(m => m.Date >= from && m.Date <= to)
                    .GroupBy(m => m.Date)
                    .Select(m => new InstructorMetricsDto
                    {
                        Label = m.Key.ToString(),
                        EnrollmentCount = m.Sum(x => x.EnrollmentCount),
                        Revenue = m.Sum(x => x.Revenue),
                        RefundCount = m.Sum(x => x.RefundCount),
                    });
            }
            else if (dateScope == DateScope.Year)
            {
                 from = to.AddYears(-1);

                return query.Where(m => m.Date >= from && m.Date <= to)
                    .GroupBy(m => new { m.Date.Year, m.Date.Month })
                    .Select(m => new InstructorMetricsDto
                    {
                        Label = $"{m.Key.Month}/{m.Key.Year}",
                        EnrollmentCount = m.Sum(x => x.EnrollmentCount),
                        Revenue = m.Sum(x => x.Revenue),
                        RefundCount = m.Sum(x => x.RefundCount),
                    });
            }
            else
            {
                return query
                    .GroupBy(m => new { m.Date.Year, m.Date.Month })
                    .Select(m => new InstructorMetricsDto
                    {
                        Label = $"{m.Key.Month}/{m.Key.Year}",
                        EnrollmentCount = m.Sum(x => x.EnrollmentCount),
                        Revenue = m.Sum(x => x.Revenue),
                        RefundCount = m.Sum(x => x.RefundCount),
                    });
            }
        }
    }
}
