using MicroShop.View.Models.HttpClients;
using MicroShop.View.Models.HttpMessageHandlers;

namespace MicroShop.View.Models.Extensions
{
    public static class HttpClientExtentions
    {
        public static void AddHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

            builder.Register<ICatalogService, CatalogService>();
            builder.Register<ICartService, CartService>();
            builder.Register<IOrdersService, OrdersService>();
        }

        private static void Register<TInterface, TImplementation>(this WebApplicationBuilder builder)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            builder.Services.AddHttpClient<TInterface, TImplementation>((IServiceProvider sp, HttpClient client) =>
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
