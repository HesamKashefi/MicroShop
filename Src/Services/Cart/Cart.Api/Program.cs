using Cart.Api.Events;
using Cart.Api.Grpc;
using Cart.Api.Services;
using Common;
using Common.Options;
using StackExchange.Redis;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();
    builder.Services.AddGrpc();
    builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!, "redis", tags: new[] { "redis" });

    builder.Services.AddScoped<ICartService, CartService>();
    builder.AddEventHandlers(typeof(ProductPriceUpdatedHandler).Assembly);

    builder.Services.AddScoped<IConnectionMultiplexer>(x =>
    {
        return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
    });
    builder.Services.AddScoped(x =>
    {
        var cm = x.GetRequiredService<IConnectionMultiplexer>();
        return cm.GetDatabase();
    });

    var app = builder.Build();

    app.MapGrpcService<GrpcCartService>();
    // Configure the HTTP request pipeline.
    app.UseDefaultPipeline();

    app.ConfigureEventBus(new UseEventBusOptions()
        .Subscribe<ProductPriceUpdated, ProductPriceUpdatedHandler>()
    );

    await app.RunAsync();
}, "Cart.Api");
