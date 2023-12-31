﻿using Common.Options;
using Common.Services;
using EventBus.Core;
using EventBus.RabbitMq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using OpenIddict.Validation.AspNetCore;
using RabbitMQ.Client;
using System.Reflection;

namespace Common
{
    public static class Extensions
    {
        public static async Task RunInLoggerAsync(Func<Task> action, string loggerName)
        {
            LogManager.Setup().LoadConfigurationFromAppSettings();
            LogManager.Configuration.Variables["AppName"] = loggerName;
            LogManager.ReconfigExistingLoggers();
            var logger = LogManager.GetLogger(loggerName);

            logger.Debug("Init Main");

            try
            {
                await action.Invoke();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static void AddEssentialServiceDefaults(this WebApplicationBuilder builder)
        {
            builder.Host.UseNLog();

            builder.Configuration.AddJsonFile("/src/projectSettings.json", false);
            builder.Configuration.AddEnvironmentVariables();

            var urlsConfiguration = builder.Configuration.GetSection("Urls");
            builder.Services.Configure<Urls>(urlsConfiguration);

            builder.Services.AddHealthChecks();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            if (builder.Environment.IsDevelopment())
            {
                var urls = urlsConfiguration.Get<Urls>()!;
                builder.Services.AddCors(x =>
                {
                    x.AddDefaultPolicy(p =>
                    {
                        p.WithOrigins(urls.Admin.TrimEnd('/'), urls.AdminLocalSpa.TrimEnd('/'), urls.View.TrimEnd('/'), urls.Identity.TrimEnd('/'), urls.OrdersSignalR.TrimEnd('/'))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });
            }
        }

        public static void AddServiceDefaults(this WebApplicationBuilder builder, string serviceName)
        {
            builder.AddEssentialServiceDefaults();

            builder.AddDefaultAuthentication();
            builder.AddEventBus(serviceName);
        }

        public static void AddDefaultAuthentication(this WebApplicationBuilder builder)
        {
            var issuer = builder.Configuration.GetValue<string>("Urls:Identity");

            if (string.IsNullOrEmpty(issuer)) return;

            // Register the OpenIddict validation components.
            builder.Services.AddOpenIddict()
                .AddValidation(options =>
                {
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

            builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            builder.Services.AddScoped<IUserService, UserService>();
        }

        public static void AddEventBus(this WebApplicationBuilder builder, string serviceName)
        {
            builder.Services.AddSingleton<IConnectionFactory>(x =>
            {
                return new ConnectionFactory
                {
                    HostName = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost",
                    DispatchConsumersAsync = true
                };
            });
            builder.Services.AddSingleton<IEventBusSubscriptionManager, DefaultEventBusSubscriptionManager>();
            builder.Services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQConnection>();
            builder.Services.AddSingleton<string>(serviceName);
            builder.Services.AddSingleton<IEventBus, RabbitMQBus>();
        }

        /// <summary>
        /// Registers event handlers with DI
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static void AddEventHandlers(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
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
            var connection = scope.ServiceProvider.GetRequiredService<IRabbitMQPersistentConnection>();

            var method = typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe))!;
            foreach (var pair in o.Handlers)
            {
                foreach (var handler in pair.Value)
                {
                    var genericMethod = method.MakeGenericMethod(pair.Key, handler);
                    genericMethod.Invoke(bus, Array.Empty<object>());
                }
            }

            Task.Delay(TimeSpan.FromSeconds(10))
                .ContinueWith((t) =>
                {
                    connection.TryConnect();
                });
        }


        public static void UseDefaultPipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultHealthChecks();

            app.MapControllers();
            app.MapRazorPages();
        }

        public static void MapDefaultHealthChecks(this IEndpointRouteBuilder routes)
        {
            routes.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}