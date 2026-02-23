using FraudDetection.Modules.Customers.Domain;
using FraudDetection.Modules.Customers.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly FraudDbContext _context;

    public CustomerRepository(FraudDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}