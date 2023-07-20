using Common;
using Identity.Api.Data;
using Identity.Api.Extensions;
using Microsoft.EntityFrameworkCore;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();
    builder.Services.AddRazorPages();

    builder.Services.AddAuthentication()
        .AddCookie(c =>
        {
            c.LoginPath = "/login";
        });

    builder.Services.AddDbContext<IdentityContext>(c =>
    {
        c.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

        c.UseOpenIddict();
    });

    builder.Services.AddOpenIddictConfig(builder.Configuration);

    var app = builder.Build();

    app.UseDefaultPipeline();
    app.MapRazorPages();

    await app.SeedDataAsync();

    await app.RunAsync();
}, "Identity.Api");
