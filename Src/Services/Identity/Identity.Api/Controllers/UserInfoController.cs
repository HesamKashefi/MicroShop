using Identity.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly IdentityContext _identityContext;

        public UserInfoController(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            var userName = User.Identity!.Name!;
            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user is null)
            {
                return NotFound();
            }
            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                [Claims.Subject] = user.Id.ToString(),
                [Claims.Name] = user.Username

            };
            return Ok(claims);
        }
    }
}
