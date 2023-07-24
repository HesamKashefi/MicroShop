using Catalog.Api.Extensions;
using Catalog.Api.Protos;
using Catalog.Application.Queries;
using Common;
using Common.Options;
using MongoDB.Driver;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetConnectionString("Mongo")!, name: "mongodb", tags: new[] { "mongodb" });

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