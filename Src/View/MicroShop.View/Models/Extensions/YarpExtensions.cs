using Common.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Forwarder;

namespace MicroShop.View.Models.Extensions
{
    public static class YarpExtensions
    {
        public static void MapSignalRForwarder(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var options = scope.ServiceProvider.GetRequiredService<IOptions<Urls>>();
            var httpClient = new HttpMessageInvoker(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
            app.MapForwarder("/hubs/orders/{**catch-all}", options.Value.OrdersSignalR, new ForwarderRequestConfig(), new AuthTransformer(), httpClient);
        }

        public class AuthTransformer : HttpTransformer
        {
            public override async ValueTask TransformRequestAsync(HttpContext httpContext, HttpRequestMessage proxyRequest, string destinationPrefix, CancellationToken cancellationToken)
            {
                var token = await httpContext.GetTokenAsync("access_token");
                if (token is not null)
                {
                    proxyRequest.Headers.Authorization = new("Bearer", token);
                }
                await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);
            }
        }
    }
}
