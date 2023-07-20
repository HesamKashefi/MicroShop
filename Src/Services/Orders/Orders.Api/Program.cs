using Common;
using Common.Options;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();

    builder.AddEventHandlers(typeof(Program).Assembly);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseDefaultPipeline();

    app.ConfigureEventBus(new UseEventBusOptions());

    await app.RunAsync();
});
