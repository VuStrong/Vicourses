using Microsoft.EntityFrameworkCore;
using PaymentService.API.Infrastructure.EntityTypeConfigurations;
using PaymentService.API.Models;

namespace PaymentService.API.Infrastructure
{
    public class PaymentServiceDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BatchPayout> BatchPayouts { get; set; }

        public PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BatchPayoutEntityTypeConfiguration());
        }
    }
}
