using FraudDetection.Modules.Customers.Domain;

namespace FraudDetection.Modules.Customers.Domain.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
}