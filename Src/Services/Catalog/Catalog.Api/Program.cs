using Catalog.Api.Extensions;
using Catalog.Api.Grpc;
using Catalog.Application.Models;
using Catalog.Application.Queries;
using Common;
using Common.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using RabbitMQ.Client;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults("CatalogAPI");
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddMongoDb(builder.Configuration.GetConnectionString("Mongo")!, name: "mongodb", tags: new[] { "mongodb" })
    .AddRabbitMQ(c => c.GetRequiredService<IConnectionFactory>(), name: "rabbitmq", tags: new[] { "rabbitmq" });

    builder.Services.Configure<PictureFileSettings>(builder.Configuration.GetSection("Urls"));

    builder.Services.AddGrpc();
    builder.AddEventHandlers(typeof(Program).Assembly);

    builder.Services.AddScoped<IMongoDatabase>(c =>
    {
        var cs = builder.Configuration.GetConnectionString("Mongo");
        var client = new MongoClient(cs);
        return client.GetDatabase("ProductsDb");
    });
    builder.Services.AddMediatR(c =>
    {
        c.RegisterServicesFromAssemblyContaining(typeof(GetProductsQuery));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseDefaultPipeline();

    app.MapGrpcService<MyCatalogService>();
    app.ConfigureEventBus(new UseEventBusOptions());

    await app.SeedDatabaseAsync();

    await app.RunAsync();
}, "Catalog.Api");