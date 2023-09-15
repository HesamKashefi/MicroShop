using Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Payments.Api.Events;
using Payments.Api.Models;
using RabbitMQ.Client;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults("Payments.Api");
    builder.AddEventHandlers(typeof(OrderStatusChangedToSubmittedEvent).Assembly);

    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("Default")!, name: "sqlserver", tags: new[] { "sqlserver" })
    .AddRabbitMQ(c => c.GetRequiredService<IConnectionFactory>(), name: "rabbitmq", tags: new[] { "rabbitmq" });

    builder.Services.Configure<PaymentsSettings>(builder.Configuration.GetSection("PaymentsSettings"));

    var app = builder.Build();

    app.UseDefaultPipeline();
    app.ConfigureEventBus(new Common.Options.UseEventBusOptions()
        .Subscribe<OrderStatusChangedToSubmittedEvent, OrderStatusChangedToSubmittedEventHandler>());

    await app.RunAsync();
}, "Payments.Api");