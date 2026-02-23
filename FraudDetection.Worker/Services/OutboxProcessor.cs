using FraudDetection.Infrastructure.Persistence;
using FraudDetection.Infrastructure.Persistence.Outbox;
using FraudDetection.Modules.Customers.Domain.Events;
using FraudDetection.Worker.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace FraudDetection.Worker.Services;

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
                    var type = Type.GetType(outboxEvent.EventType);

                    if (type == typeof(CustomerCreatedDomainEvent))
                    {
                        var domainEvent = JsonSerializer.Deserialize<CustomerCreatedDomainEvent>(
                            outboxEvent.Payload);

                        var handler = scope.ServiceProvider.GetRequiredService<CustomerCreatedHandler>();

                        await handler.Handle(domainEvent!, stoppingToken);
                    }

                    outboxEvent.MarkAsProcessed();
                }
                catch
                {
                    outboxEvent.IncrementRetry();
                }
            }

            await context.SaveChangesAsync(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }
}