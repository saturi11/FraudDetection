using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FraudDetection.Modules.Customers.Domain;
using FraudDetection.BuildingBlocks.Domain;
using FraudDetection.API.Infrastructure.Persistence.Outbox;

namespace FraudDetection.API.Infrastructure.Persistence;

public class FraudDbContext : DbContext
{
    public FraudDbContext(DbContextOptions<FraudDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerMetrics> CustomerMetrics => Set<CustomerMetrics>();
    public DbSet<OutboxEvent> OutboxEvents => Set<OutboxEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FraudDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var events = x.DomainEvents;
                x.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var outboxEvent = new OutboxEvent(
                domainEvent.GetType().FullName!,
                JsonSerializer.Serialize(domainEvent),
                domainEvent.OccurredOn);

            await OutboxEvents.AddAsync(outboxEvent, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
