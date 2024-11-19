using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatisticsService.API.Models;

namespace StatisticsService.API.Infrastructure.EntityTypeConfigurations
{
    public class InstructorMetricEntityTypeConfiguration : IEntityTypeConfiguration<InstructorMetric>
    {
        public void Configure(EntityTypeBuilder<InstructorMetric> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => new { e.InstructorId, e.CourseId, e.Date }).IsUnique();
            builder.HasIndex(e => new { e.InstructorId, e.Date });
        }
    }
}
