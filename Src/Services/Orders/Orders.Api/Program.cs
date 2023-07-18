using Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.AddDefaultAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultPipeline();

app.Run();
