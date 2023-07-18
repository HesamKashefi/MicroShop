using Catalog.Api.Extensions;
using Catalog.Application.Queries;
using Common;
using Common.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.AddDefaultAuthentication();
builder.AddEventBus();
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
builder.Services.AddSwaggerGen();

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

await app.SeedDatabaseAsync();

app.ConfigureEventBus(new UseEventBusOptions());

app.Run();
