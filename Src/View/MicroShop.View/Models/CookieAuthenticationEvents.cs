using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;

namespace MicroShop.View.Models
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var token = context.Properties.GetTokenValue("access_token");
            if (token is not null)
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.ReadToken(token).ValidTo < DateTime.Now)
                {
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.RejectPrincipal();
                }
            }
            await base.ValidatePrincipal(context);
        }
    }
}
