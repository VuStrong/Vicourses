using Microsoft.EntityFrameworkCore;
using StatisticsService.API.Infrastructure.EntityTypeConfigurations;
using StatisticsService.API.Models;

namespace StatisticsService.API.Infrastructure
{
    public class StatisticsServiceDbContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<InstructorMetric> InstructorMetrics { get; set; }
        public DbSet<AdminMetric> AdminMetrics { get; set; }
        public DbSet<User> Users { get; set; }

        public StatisticsServiceDbContext(DbContextOptions<StatisticsServiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorMetricEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdminMetricEntityTypeConfiguration());
        }
    }
}
