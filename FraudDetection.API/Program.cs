using FraudDetection.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using FraudDetection.Modules.Customers.Application.Commands.CreateCustomer;
using FraudDetection.Modules.Customers.Domain.Repositories;
using FraudDetection.API.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCustomerCommand>());

// DbContext
builder.Services.AddDbContext<FraudDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();