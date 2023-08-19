using Common;
using Common.Options;
using Microsoft.EntityFrameworkCore;
using Orders.Api.Models;
using Orders.Persistence;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();
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
