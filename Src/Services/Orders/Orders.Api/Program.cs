using Common;
using Common.Options;
using EventBus.Core;
using EventLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orders.Api.Models;
using Orders.Application.EventHandlers;
using Orders.Application.Events;
using Orders.Application.Queries;
using Orders.Domain.Contracts;
using Orders.Persistence;
using Orders.Persistence.Repositories;
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
        c.UseSqlServer(builder.Configuration.GetConnectionString("Default"), d =>
        {
            d.MigrationsAssembly(typeof(OrdersDb).Assembly.FullName!);
        });
    });
    builder.Services.AddDbContext<EventLogContext>((s, c) =>
    {
        var dbConnection = s.GetRequiredService<OrdersDb>().Database.GetDbConnection();
        c.UseSqlServer(dbConnection, d =>
        {
            d.MigrationsAssembly(typeof(EventLogContext).Assembly.FullName!);
        });
    });

    builder.AddEventHandlers(typeof(UserCheckoutStartedEvent).Assembly);
    builder.Services.AddMediatR(c =>
    {
        c.RegisterServicesFromAssemblyContaining(typeof(GetBuyerOrdersQuery));
    });
    builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
    builder.Services.AddScoped<IEventLogService, EventLogService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseDefaultPipeline();

    app.ConfigureEventBus(new UseEventBusOptions()
        .Subscribe<UserCheckoutStartedEvent, UserCheckoutStartedEventHandler>());

    using (var scope = app.Services.CreateScope())
    {
        var subscriptionManager = scope.ServiceProvider.GetRequiredService<IEventBusSubscriptionManager>();
        subscriptionManager.RegisterEventType<OrderStatusChangedToSubmittedEvent>();
    }

    await app.DbInitAsync();
    await app.RunAsync();
}, "Orders.Api");
