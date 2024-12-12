using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RatingService.API.Models;

namespace RatingService.API.Infrastructure.EntityTypeConfigurations
{
    internal class RatingEntityTypeConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.CourseId, x.UserId }).IsUnique();

            builder.HasIndex(x => new { x.InstructorId, x.CourseId });

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
