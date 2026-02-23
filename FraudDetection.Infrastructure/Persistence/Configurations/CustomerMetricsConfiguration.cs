using FraudDetection.Modules.Customers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FraudDetection.Infrastructure.Persistence.Configurations;

public class CustomerMetricsConfiguration : IEntityTypeConfiguration<CustomerMetrics>
{
    public void Configure(EntityTypeBuilder<CustomerMetrics> builder)
    {
        builder.ToTable("customer_metrics");

        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.CustomerId)
            .IsRequired();

        builder.Property(cm => cm.TotalTransactions)
            .IsRequired();

        builder.Property(cm => cm.TotalRejectedTransactions)
            .IsRequired();

        builder.Property(cm => cm.TotalAmount)
            .HasColumnType("numeric(18,2)");

        builder.Property(cm => cm.AverageTransactionAmount)
            .HasColumnType("numeric(18,2)");

        builder.Property(cm => cm.Version)
            .IsConcurrencyToken();
    }
}
