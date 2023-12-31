using Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

await Extensions.RunInLoggerAsync(async () =>
{
    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

    var builder = WebApplication.CreateBuilder(args);
    builder.AddEssentialServiceDefaults();

    // Add services to the container.
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy());
    builder.Services.AddHealthChecksUI(c =>
    {
        c.UseApiEndpointHttpMessageHandler(s =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
        });
    })
    .AddInMemoryStorage();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseHealthChecks("/hc");
    app.UseHealthChecksUI(c =>
    {
        c.UIPath = "/hc-ui";
    });

    await app.RunAsync();
}, "HealthChecksUI");
