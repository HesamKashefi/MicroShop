
using Identity.Api.Data;
using Identity.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

builder.Services.AddOpenIddictConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

await app.SeedDataAsync();

app.Run();