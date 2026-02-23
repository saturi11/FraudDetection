using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using FraudDetection.Modules.Customers.Domain;
using FraudDetection.BuildingBlocks.Domain;
using FraudDetection.Infrastructure.Persistence.Outbox;

namespace FraudDetection.Infrastructure.Persistence;

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
            .Entries()
            .Where(e => e.Entity is AggregateRoot)
            .Select(e => (AggregateRoot)e.Entity)
            .SelectMany(x =>
            {
                var events = x.DomainEvents.ToList(); 
                x.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var outboxEvent = new OutboxEvent(
                domainEvent.GetType().FullName!,
                JsonSerializer.Serialize(
                    domainEvent,
                    domainEvent.GetType()
                ),
                domainEvent.OccurredOn);

            await OutboxEvents.AddAsync(outboxEvent, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
