using Catalog.Api.Protos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using System.Globalization;

namespace ApiGateway.Extensions
{
    public static class DefaultExtensions
    {
        public static void AddReverseProxy(this WebApplicationBuilder builder)
        {
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .ConfigureHttpClient((context, handler) =>
                {
                    handler.SslOptions.RemoteCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                });
        }

        public static void AddGrpcServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddGrpcClient<CatalogService.CatalogServiceClient>((provider, options) =>
            {
                var url = builder.Configuration.GetValue<string>("Urls:Catalog")!;
                options.Address = new Uri(url);
            })
            .AddCallCredentials(async (context, meta, provider) =>
            {
                var httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext!;
                var token = await httpContext.GetTokenAsync("access_token");
                token ??= httpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "", true, CultureInfo.CurrentCulture);
                meta.Add("Authorization", "Bearer " + token);
            });
        }
    }
}
