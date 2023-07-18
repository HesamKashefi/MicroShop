using Cart.Api.Events;
using Cart.Api.Services;
using Common;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.AddDefaultAuthentication();
builder.Services.AddScoped<ICartService, CartService>();
builder.AddEventBus(c =>
{
    c.AddScoped<ProductPriceUpdatedHandler>();
});

builder.Services.AddScoped<IConnectionMultiplexer>(x =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Default")!);
});
builder.Services.AddScoped(x =>
{
    var cm = x.GetRequiredService<IConnectionMultiplexer>();
    return cm.GetDatabase();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ConfigureEventBus((bus) =>
{
    bus.Subscribe<ProductPriceUpdated, ProductPriceUpdatedHandler>();
});

app.Run();
