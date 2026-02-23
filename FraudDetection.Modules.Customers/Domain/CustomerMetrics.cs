using FraudDetection.BuildingBlocks.Domain;

namespace FraudDetection.Modules.Customers.Domain;

public sealed class CustomerMetrics : AggregateRoot
{
    public Guid CustomerId { get; private set; }

    public int TotalTransactions { get; private set; }
    public int TotalRejectedTransactions { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal AverageTransactionAmount { get; private set; }

    public int Version { get; private set; }

    private CustomerMetrics() { }

    private CustomerMetrics(Guid customerId)
        : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        TotalTransactions = 0;
        TotalRejectedTransactions = 0;
        TotalAmount = 0;
        AverageTransactionAmount = 0;
    }

    public static CustomerMetrics Create(Guid customerId)
    {
        return new CustomerMetrics(customerId);
    }

    public void RegisterTransaction(decimal amount, bool rejected)
    {
        TotalTransactions++;

        if (rejected)
            TotalRejectedTransactions++;

        TotalAmount += amount;

        AverageTransactionAmount =
            TotalTransactions == 0
                ? 0
                : TotalAmount / TotalTransactions;
    }
}
