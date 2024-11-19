using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Application.Dtos;
using StatisticsService.API.Infrastructure;
using StatisticsService.API.Models;
using StatisticsService.API.Utils;

namespace StatisticsService.API.Application.Services
{
    public class AdminDashboardDataStatistician : IAdminDashboardDataStatistician
    {
        private readonly StatisticsServiceDbContext _dbContext;

        public AdminDashboardDataStatistician(StatisticsServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AdminDashboardDataDto> GetAdminDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            var totalStudent = await _dbContext.Users.CountAsync(u => u.Role == Roles.Student, cancellationToken);
            var totalInstructor = await _dbContext.Users.CountAsync(u => u.Role == Roles.Instructor, cancellationToken);
            var totalPublishedCourse = await _dbContext.Courses.CountAsync(c => c.Status == CourseStatus.Published, cancellationToken);

            return new AdminDashboardDataDto()
            {
                TotalStudent = totalStudent,
                TotalInstructor = totalInstructor,
                TotalPublishedCourse = totalPublishedCourse,
            };
        }
    }
}
