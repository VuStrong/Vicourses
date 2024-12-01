using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.API.Models;

namespace PaymentService.API.Infrastructure.EntityTypeConfigurations
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.PaypalOrderId).IsUnique();

            builder.HasIndex(x => new { x.CreatedAt, x.Status });

            builder.HasIndex(x => new { x.Status, x.PaymentDueDate });

            builder
                .Property(p => p.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v));

            builder
                .Property(p => p.Method)
                .HasConversion(
                    v => v.ToString(),
                    v => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), v));
        }
    }
}
