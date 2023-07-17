using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace Common
{
    public static class Extensions
    {
        public static void AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the OpenIddict validation components.
            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    var issuer = configuration.GetValue<string>("Urls:Identity")!;
                    // Note: the validation handler uses OpenID Connect discovery
                    // to retrieve the address of the introspection endpoint.
                    options.SetIssuer(issuer);

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            services.AddCors();
            services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}