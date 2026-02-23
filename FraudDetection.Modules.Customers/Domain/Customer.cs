using FraudDetection.BuildingBlocks.Domain;
using FraudDetection.Modules.Customers.Domain.ValueObjects;
using FraudDetection.Modules.Customers.Domain.Events;

namespace FraudDetection.Modules.Customers.Domain;

public sealed class Customer : AggregateRoot
{
    public string Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Country Country { get; private set; } = default!;

    public bool IsBlocked { get; private set; }

    private Customer() { }

    private Customer(Guid id, string name, Email email, Country country)
        : base(id)
    {
        Name = name;
        Email = email;
        Country = country;
        IsBlocked = false;
    }

    public static Customer Create(string name, Email email, Country country)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        var customer = new Customer(Guid.NewGuid(), name, email, country);
        customer.AddDomainEvent(
        new CustomerCreatedDomainEvent(customer.Id));

        return customer;
    }

    public void Block()
    {
        IsBlocked = true;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }
}
