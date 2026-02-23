using FraudDetection.BuildingBlocks.Domain;
using FraudDetection.Infrastructure.Persistence;
using FraudDetection.Modules.Customers.Domain;
using FraudDetection.Modules.Customers.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Worker.Handlers;

public class CustomerCreatedHandler
{
    private readonly FraudDbContext _context;

    public CustomerCreatedHandler(FraudDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CustomerCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var metrics = CustomerMetrics.Create(domainEvent.CustomerId);

        await _context.CustomerMetrics.AddAsync(metrics, cancellationToken);
    }
}