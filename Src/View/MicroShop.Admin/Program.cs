using Common;
using Common.Options;
using Microsoft.Extensions.Options;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.AddEssentialServiceDefaults();
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseCors();
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseStaticFiles();
    app.UseRouting();

    app.MapDefaultHealthChecks();

    app.MapGet("/api/config", (IOptions<Urls> options) => options.Value)
    .WithName("GetConfig")
    .WithOpenApi();

    app.MapFallbackToFile("index.html");

    await app.RunAsync();
}, "MicroShop.Admin");
