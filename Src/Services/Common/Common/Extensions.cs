using EventBus.Core;
using EventBus.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using RabbitMQ.Client;

namespace Common
{
    public static class Extensions
    {
        public static void AddDefaultAuthentication(this WebApplicationBuilder builder)
        {
            // Register the OpenIddict validation components.
            builder.Services.AddOpenIddict()
                .AddValidation(options =>
                {
                    var issuer = builder.Configuration.GetValue<string>("Urls:Identity")!;
                    // Note: the validation handler uses OpenID Connect discovery
                    // to retrieve the address of the introspection endpoint.
                    options.SetIssuer(issuer);

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            builder.Services.AddCors();
            builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        public static void AddEventBus(this WebApplicationBuilder builder, Action<IServiceCollection> configureBus)
        {
            builder.Services.AddSingleton<IConnectionFactory>(x =>
            {
                return new ConnectionFactory
                {
                    HostName = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
                    DispatchConsumersAsync = true
                };
            });
            builder.Services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQConnection>();
            builder.Services.AddSingleton<IEventBus, RabbitMQBus>();
            configureBus?.Invoke(builder.Services);
        }

        public static void ConfigureEventBus(this IApplicationBuilder builder, Action<IEventBus> configureBus)
        {
            var scope = builder.ApplicationServices.CreateScope();
            var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            configureBus?.Invoke(bus);
        }
    }
}