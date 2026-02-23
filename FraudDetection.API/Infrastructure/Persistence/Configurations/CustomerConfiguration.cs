using FraudDetection.Modules.Customers.Domain;
using FraudDetection.Modules.Customers.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FraudDetection.API.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);
        });

        builder.OwnsOne(c => c.Country, country =>
        {
            country.Property(c => c.Code)
                .HasColumnName("country")
                .IsRequired()
                .HasMaxLength(2);
        });

        builder.Property(c => c.IsBlocked)
            .IsRequired();
    }
}
