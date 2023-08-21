using Common;
using Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orders.Api.Models;
using Orders.Persistence;
using RabbitMQ.Client;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("Default")!, name: "sqlserver", tags: new[] { "sqlserver" })
    .AddRabbitMQ(c => c.GetRequiredService<IConnectionFactory>(), name: "rabbitmq", tags: new[] { "rabbitmq" });
    builder.Services.AddDbContext<OrdersDb>(c =>
    {
        c.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });

    builder.AddEventHandlers(typeof(Program).Assembly);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseDefaultPipeline();

    app.ConfigureEventBus(new UseEventBusOptions());

    await app.DbInitAsync();
    await app.RunAsync();
}, "Orders.Api");
