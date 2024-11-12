using DiscountService.API.Infrastructure.EntityTypeConfigurations;
using DiscountService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.API.Infrastructure
{
    public class DiscountServiceDbContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DiscountServiceDbContext(DbContextOptions<DiscountServiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CouponEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
        }
    }
}
