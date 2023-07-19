using Common.Options;
using EventBus.Core;
using EventBus.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using RabbitMQ.Client;
using System.Reflection;

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
                    options.UseSystemNetHttp(c =>
                    {
                        c.ConfigureHttpClient(options =>
                        {
                            options.BaseAddress = new Uri(issuer);
                        });

                        c.ConfigureHttpClientHandler(options =>
                        {
                            options.ServerCertificateCustomValidationCallback = (message, cert, chain, err) => true;
                        });
                    });

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            builder.Services.AddCors();
            builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        public static void AddEventBus(this WebApplicationBuilder builder, params Assembly[] assemblies)
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
            builder.Services.Scan(s =>
            {
                s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .WithTransientLifetime();
            });
        }

        public static void ConfigureEventBus(this IApplicationBuilder builder, UseEventBusOptions options)
        {
            var o = options ?? throw new ArgumentNullException(nameof(options));

            var scope = builder.ApplicationServices.CreateScope();
            var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var method = typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe))!;
            foreach (var pair in o.Handlers)
            {
                foreach (var handler in pair.Value)
                {
                    var genericMethod = method.MakeGenericMethod(pair.Key, handler);
                    genericMethod.Invoke(bus, Array.Empty<object>());
                }
            }
        }


        public static void UseDefaultPipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}