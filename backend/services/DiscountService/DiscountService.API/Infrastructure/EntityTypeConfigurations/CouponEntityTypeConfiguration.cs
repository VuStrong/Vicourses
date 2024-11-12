using DiscountService.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscountService.API.Infrastructure.EntityTypeConfigurations
{
    public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasIndex(x => new { x.CourseId, x.ExpiryDate });

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
