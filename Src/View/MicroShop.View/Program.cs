using Common;
using MicroShop.View.Models.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

await Extensions.RunInLoggerAsync(async () =>
{
    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddEssentialServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

    builder.AddViewDefaultAuthentication();
    builder.AddHttpClients();

    var app = builder.Build();

    app.UseDefaultPipeline();

    await app.RunAsync();
}, "MicroShop.View");