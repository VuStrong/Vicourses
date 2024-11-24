using Microsoft.EntityFrameworkCore;
using PaymentService.API.Infrastructure.EntityTypeConfigurations;
using PaymentService.API.Models;

namespace PaymentService.API.Infrastructure
{
    public class PaymentServiceDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
        }
    }
}
