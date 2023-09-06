using ApiGateway.Extensions;
using Common;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults("ApiGateway");
    builder.AddReverseProxy();
    builder.AddGrpcServices();

    var app = builder.Build();

    app.UseDefaultPipeline();

    app.MapReverseProxy();
    await app.RunAsync();
}, "ApiGateway");