using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StatisticsService.API.Application.Dtos;
using StatisticsService.API.Infrastructure;
using StatisticsService.API.Models;
using StatisticsService.API.Utils;
using System.Text.Json;

namespace StatisticsService.API.Application.Services
{
    public class AdminDashboardDataStatistician : IAdminDashboardDataStatistician
    {
        private readonly StatisticsServiceDbContext _dbContext;
        private readonly IDistributedCache _cache;

        public AdminDashboardDataStatistician(StatisticsServiceDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<AdminDashboardDataDto> GetAdminDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            var cachedKey = $"admin-dashboard";
            var cachedData = await _cache.GetStringAsync(cachedKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<AdminDashboardDataDto>(cachedData)!;
            }

            var totalStudent = await _dbContext.Users.CountAsync(u => u.Role == Roles.Student, cancellationToken);
            var totalInstructor = await _dbContext.Users.CountAsync(u => u.Role == Roles.Instructor, cancellationToken);
            var totalPublishedCourse = await _dbContext.Courses.CountAsync(c => c.Status == CourseStatus.Published, cancellationToken);

            var to = DateOnly.FromDateTime(DateTime.Today);
            var from = to.AddMonths(-1);

            var monthMetrics = await _dbContext.AdminMetrics
                .Where(m => m.Date >= from && m.Date <= to)
                .Select(m => new AdminMetricsDto
                {
                    Label = m.Date.ToString(),
                    Revenue = m.Revenue,
                })
                .ToListAsync(cancellationToken);
            var totalMonthRevenue = monthMetrics.Sum(m => m.Revenue);

            var data = new AdminDashboardDataDto
            {
                TotalStudent = totalStudent,
                TotalInstructor = totalInstructor,
                TotalPublishedCourse = totalPublishedCourse,
                TotalMonthRevenue= totalMonthRevenue,
                Metrics = monthMetrics,
            };

            await _cache.SetStringAsync(cachedKey, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(10)
            });

            return data;
        }

        public async Task<List<AdminMetricsDto>> GetAdminMetricsAsync(DateScope dateScope, CancellationToken cancellationToken = default)
        {
            DateOnly to = DateOnly.FromDateTime(DateTime.Today);
            DateOnly? from = null;

            var query = _dbContext.AdminMetrics.AsQueryable();

            if (dateScope == DateScope.Week)
            {
                from = to.AddDays(-7);
            }
            else if (dateScope == DateScope.Month)
            {
                from = to.AddMonths(-1);
            }
            else if (dateScope == DateScope.Year)
            {
                from = to.AddYears(-1);
            }

            if (from != null)
            {
                query = query.Where(m => m.Date >= from.Value && m.Date <= to);
            }

            List<AdminMetricsDto> metrics;
            if (dateScope == DateScope.Week || dateScope == DateScope.Month)
            {
                metrics = await query.Select(m => new AdminMetricsDto
                {
                    Label = m.Date.ToString(),
                    Revenue = m.Revenue
                })
                .ToListAsync();
            }
            else
            {
                metrics = await query.GroupBy(m => new { m.Date.Year, m.Date.Month })
                    .Select(m => new AdminMetricsDto
                    {
                        Label = $"{m.Key.Month}/{m.Key.Year}",
                        Revenue = m.Sum(x => x.Revenue),
                    })
                    .ToListAsync();
            }

            return metrics;
        }
    }
}
