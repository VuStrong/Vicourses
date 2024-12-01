using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.API.Models;

namespace PaymentService.API.Infrastructure.EntityTypeConfigurations
{
    public class BatchPayoutEntityTypeConfiguration : IEntityTypeConfiguration<BatchPayout>
    {
        public void Configure(EntityTypeBuilder<BatchPayout> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Date);
        }
    }
}
