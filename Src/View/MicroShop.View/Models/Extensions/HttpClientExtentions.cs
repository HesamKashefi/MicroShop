using MicroShop.View.Models.HttpClients;
using MicroShop.View.Models.HttpMessageHandlers;

namespace MicroShop.View.Models.Extensions
{
    public static class HttpClientExtentions
    {
        public static void AddHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

            builder.Services.AddHttpClient<ICatalogService, CatalogService>((sp, client) =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:Apigateway")!);
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
            .ConfigurePrimaryHttpMessageHandler(ConfigureHttpMessageHandler);


            builder.Services.AddHttpClient<ICartService, CartService>((sp, client) =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:Apigateway")!);
            })
            .AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
            .ConfigurePrimaryHttpMessageHandler(ConfigureHttpMessageHandler);
        }


        private static HttpMessageHandler ConfigureHttpMessageHandler(IServiceProvider sp)
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        }
    }
}
