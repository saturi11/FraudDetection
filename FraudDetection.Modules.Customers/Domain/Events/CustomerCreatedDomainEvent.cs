using FraudDetection.BuildingBlocks.Domain;

namespace FraudDetection.Modules.Customers.Domain.Events;

public sealed class CustomerCreatedDomainEvent : DomainEvent
{
    public Guid CustomerId { get; }

    public CustomerCreatedDomainEvent(Guid customerId)
    {
        CustomerId = customerId;
    }
}