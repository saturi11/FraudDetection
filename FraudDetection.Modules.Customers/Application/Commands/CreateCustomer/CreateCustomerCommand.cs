using FraudDetection.BuildingBlocks.Application;
using MediatR;

namespace FraudDetection.Modules.Customers.Application.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string Email,
    string Country
) : IRequest<Result<Guid>>;
