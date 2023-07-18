using Cart.Api.Events;
using Cart.Api.Services;
using Common;
using Common.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.AddDefaultAuthentication();
builder.Services.AddScoped<ICartService, CartService>();
builder.AddEventBus(typeof(ProductPriceUpdatedHandler).Assembly);

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

// Configure the HTTP request pipeline.
app.UseDefaultPipeline();

app.ConfigureEventBus(new UseEventBusOptions()
    .Subscribe<ProductPriceUpdated, ProductPriceUpdatedHandler>()
);

app.Run();
