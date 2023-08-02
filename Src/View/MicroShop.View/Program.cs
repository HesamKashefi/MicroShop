using Common;
using MicroShop.View.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

await Extensions.RunInLoggerAsync(async () =>
{
    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

    builder.AddViewDefaultAuthentication();

    var app = builder.Build();

    app.UseDefaultPipeline();

    await app.RunAsync();
}, "MicroShop.View");