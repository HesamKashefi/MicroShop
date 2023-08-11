using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace MicroShop.View.Controller
{
    [Route("[Controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("SignIn")]
        public ActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/",
                IsPersistent = true
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //signout from identity.api
            //await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
