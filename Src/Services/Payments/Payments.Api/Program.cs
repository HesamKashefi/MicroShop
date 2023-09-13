using Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults("Payments.Api");
    builder.AddEventHandlers(typeof(Program).Assembly);

    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("Default")!, name: "sqlserver", tags: new[] { "sqlserver" })
    .AddRabbitMQ(c => c.GetRequiredService<IConnectionFactory>(), name: "rabbitmq", tags: new[] { "rabbitmq" });

    var app = builder.Build();

    app.UseDefaultPipeline();
    app.ConfigureEventBus(new Common.Options.UseEventBusOptions());

    await app.RunAsync();
}, "Payments.Api");