﻿using Identity.Api.Data;
using OpenIddict.Abstractions;

namespace Identity.Api.Extensions
{
    public static class DataExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            db.Database.EnsureCreated();

            if (!db.Users.Any())
            {
                db.Users.Add(Identity.Api.Entities.User.Create("hesam", "hesam"));
                db.SaveChanges();
            }

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "MicroShop",
                ClientSecret = "38567b43-tebe-18ce-8ba8-ab57356d4dga",
                DisplayName = "MicroShop Web Application",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Logout,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles
                }
            };
            foreach (var address in new string[] { "http://localhost:5000" })
            {
                descriptor.PostLogoutRedirectUris.Add(new Uri(address));
                descriptor.RedirectUris.Add(new Uri(address + "/login-callback"));
            }

            descriptor.RedirectUris.Add(new Uri("https://oauth.pstmn.io/v1/callback"));

            var client = await manager.FindByClientIdAsync(descriptor.ClientId);
            if (client == null)
            {
                await manager.CreateAsync(descriptor);
            }
            else if (client is OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication d)
            {
                descriptor.Type = d.Type;
                await manager.UpdateAsync(client, descriptor);
            }

            return app;
        }
    }
}