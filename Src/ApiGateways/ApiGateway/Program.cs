using Common;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();
    builder.Services.AddRazorPages();

    var app = builder.Build();

    app.UseDefaultPipeline();
    app.MapRazorPages();

    await app.RunAsync();
});