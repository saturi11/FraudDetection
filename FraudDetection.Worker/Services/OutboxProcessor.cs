using FraudDetection.Infrastructure.Persistence;
using FraudDetection.Infrastructure.Persistence.Outbox;
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
                    Console.WriteLine($"Processando evento: {outboxEvent.EventType}");

                    // Aqui futuramente vamos desserializar dinamicamente
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