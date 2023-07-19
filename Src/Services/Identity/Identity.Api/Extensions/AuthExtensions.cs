using Identity.Api.Data;
using OpenIddict.Abstractions;

namespace Identity.Api.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddOpenIddictConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenIddict(o =>
            {
                o.AddCore(c =>
                {
                    c.UseEntityFrameworkCore()
                    .UseDbContext<IdentityContext>();
                });

                o.AddServer(c =>
                {
                    c.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()

                    //disable https
                    .DisableTransportSecurityRequirement();

                    c.RegisterScopes(
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Roles,
                        OpenIddictConstants.Scopes.OfflineAccess);

                    c.SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token");

                    c.AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow();

                    c.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                    c.SetIssuer(configuration.GetValue<string>("Urls:Identity")!);

                    c.DisableAccessTokenEncryption();
                });
            });
            return services;
        }
    }
}
