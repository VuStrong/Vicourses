using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatisticsService.API.Models;

namespace StatisticsService.API.Infrastructure.EntityTypeConfigurations
{
    public class AdminMetricEntityTypeConfiguration : IEntityTypeConfiguration<AdminMetric>
    {
        public void Configure(EntityTypeBuilder<AdminMetric> builder)
        {
            builder.HasKey(x => x.Date);
        }
    }
}
