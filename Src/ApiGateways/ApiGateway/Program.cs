using Common;
using System.Net;

ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

await Extensions.RunInLoggerAsync(async () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
        .ConfigureHttpClient((context, handler) =>
        {
            handler.SslOptions.RemoteCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        });
    var app = builder.Build();
    app.MapReverseProxy();
    await app.RunAsync();
}, "ApiGateway");