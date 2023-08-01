using Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

await Extensions.RunInLoggerAsync(async () =>
{
    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.AddServiceDefaults();
    builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

    builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddCookie()
    .AddOpenIdConnect(c =>
    {
        c.Authority = builder.Configuration.GetValue<string>("Urls:Identity");
        c.ClientId = "MicroShop";
        c.ClientSecret = "38567b43-tebe-18ce-8ba8-ab57356d4dga";
        c.SaveTokens = true;
        c.ResponseType = OpenIdConnectResponseType.Code;
        c.RequireHttpsMetadata = false;
        c.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        c.BackchannelHttpHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    });

    builder.Services.AddScoped(sp => new HttpClient(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    })
    {
        BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:Apigateway")!),
    });

    var app = builder.Build();

    app.UseDefaultPipeline();

    await app.RunAsync();
}, "MicroShop.View");