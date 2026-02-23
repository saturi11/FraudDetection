using FraudDetection.BuildingBlocks.Application;
using FraudDetection.Modules.Customers.Domain;
using FraudDetection.Modules.Customers.Domain.Repositories;
using FraudDetection.Modules.Customers.Domain.ValueObjects;
using MediatR;

namespace FraudDetection.Modules.Customers.Application.Commands.CreateCustomer;

public sealed class CreateCustomerHandler
    : IRequestHandler<CreateCustomerCommand, Result<Guid>>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        var country = Country.Create(request.Country);

        var customer = Customer.Create(
            request.Name,
            email,
            country);

        await _repository.AddAsync(customer, cancellationToken);

        return Result<Guid>.Success(customer.Id);
    }
}