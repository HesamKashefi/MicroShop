using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MicroShop.View.Models
{
    public static class AuthExtensions
    {
        public static void AddViewDefaultAuthentication(this WebApplicationBuilder builder)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = builder.Configuration.GetValue<string>("Urls:Identity");
                options.ClientId = "MicroShop";
                options.ClientSecret = "38567b43-tebe-18ce-8ba8-ab57356d4dga";
                options.SaveTokens = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.RequireHttpsMetadata = false;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.BackchannelHttpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
                options.Events.OnUserInformationReceived = ctx =>
                {
                    Console.WriteLine();
                    Console.WriteLine("Claims from the ID token");
                    if (ctx.Principal is not null)
                    {
                        foreach (var claim in ctx.Principal.Claims)
                        {
                            Console.WriteLine($"{claim.Type} - {claim.Value}");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Claims from the UserInfo endpoint");
                    foreach (var property in ctx.User.RootElement.EnumerateObject())
                    {
                        Console.WriteLine($"{property.Name} - {property.Value}");
                    }
                    return Task.CompletedTask;
                };
            });



            // authenticated http client
            builder.Services.AddScoped(sp =>
            {
                using var scope = sp.CreateScope();
                var accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var handler = new AuthHttpClientHandler(accessor)
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:Apigateway")!)
                };
                if (accessor.HttpContext is not null)
                {
                    var token = accessor.HttpContext.GetTokenAsync("access_token").Result;
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                    }
                }
                return client;
            });
        }
    }
}
