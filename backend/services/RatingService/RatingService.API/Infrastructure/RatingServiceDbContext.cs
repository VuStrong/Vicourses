using Microsoft.EntityFrameworkCore;
using RatingService.API.Infrastructure.EntityTypeConfigurations;
using RatingService.API.Models;

namespace RatingService.API.Infrastructure
{
    public class RatingServiceDbContext : DbContext
    {
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public RatingServiceDbContext(DbContextOptions<RatingServiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RatingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnrollmentEntityTypeConfiguration());
        }
    }
}
