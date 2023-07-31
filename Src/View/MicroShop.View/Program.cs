using Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());
    builder.Services.AddScoped(sp => new HttpClient(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    })
    {
        BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:Apigateway")!),
    });

    var app = builder.Build();

    app.UseDefaultPipeline();

    await app.RunAsync();
}, "MicroShop.View");
