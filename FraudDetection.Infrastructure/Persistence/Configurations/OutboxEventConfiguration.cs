using FraudDetection.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FraudDetection.Infrastructure.Persistence.Configurations;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
	public void Configure(EntityTypeBuilder<OutboxEvent> builder)
	{
		builder.ToTable("outbox_events");

		builder.HasKey(o => o.Id);

		builder.Property(o => o.EventType)
			.IsRequired();

		builder.Property(o => o.Payload)
			.IsRequired();

		builder.Property(o => o.OccurredOn)
			.IsRequired();
	}
}