using Identity.Api.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
                    .EnableUserinfoEndpointPassthrough()

                    //disable https
                    .DisableTransportSecurityRequirement();

                    c.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, Scopes.OfflineAccess);
                    c.RegisterClaims(Claims.Subject, Claims.Name);

                    c.SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo");

                    c.AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow();

                    c.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                    c.SetIssuer(configuration.GetValue<string>("Urls:Identity")!);

                    c.DisableAccessTokenEncryption();
                });

                o.AddValidation(options =>
                 {
                     options.UseLocalServer();
                     options.UseAspNetCore();
                 });
            });
            return services;
        }
    }
}
