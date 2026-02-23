using FraudDetection.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddDbContext<FraudDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<FraudDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=frauddb;Username=frauduser;Password=fraudpass"));

builder.Services.AddHostedService<OutboxProcessor>();

var host = builder.Build();
host.Run();