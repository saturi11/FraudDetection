using FraudDetection.Infrastructure.Persistence;
using FraudDetection.Infrastructure.Persistence.Outbox;
using FraudDetection.Modules.Customers.Domain;
using FraudDetection.Modules.Customers.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxProcessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FraudDbContext>();

            var events = await context.OutboxEvents
                .Where(e => e.ProcessedOn == null)
                .OrderBy(e => e.OccurredOn)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var outboxEvent in events)
            {
                try
                {
                    if (outboxEvent.EventType ==
                        typeof(CustomerCreatedDomainEvent).FullName)
                    {
                        var domainEvent =
                            JsonSerializer.Deserialize<CustomerCreatedDomainEvent>(
                                outboxEvent.Payload);

                        if (domainEvent != null)
                        {
                            var metrics = CustomerMetrics.Create(domainEvent.CustomerId);

                            await context.CustomerMetrics
                                .AddAsync(metrics, stoppingToken);
                        }
                    }

                    outboxEvent.MarkAsProcessed();
                }
                catch
                {
                    outboxEvent.IncrementRetry();
                }
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(2000, stoppingToken);
        }
    }
}