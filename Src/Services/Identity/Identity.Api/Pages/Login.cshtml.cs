using Identity.Api.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Api.Pages;

public record LoginViewModel(string Username, string Password);
public class LoginModel : PageModel
{
    private readonly IdentityContext _identityContext;

    [BindProperty]
    public LoginViewModel LoginViewModel { get; set; } = new("", "");


    public LoginModel(IdentityContext identityContext)
    {
        _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
    }

    public void OnGet([FromQuery] string? returnUrl = null)
    {
        ViewData["returnUrl"] = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync([FromQuery] string? returnUrl = null)
    {
        var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Username == LoginViewModel.Username);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid Credentials");
            return Page();
        }

        if (!user.CheckPassword(LoginViewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid Credentials");
            return Page();
        }
        var claims = new List<Claim>()
        {
            new("sub", user.Id.ToString()),
            new("name", user.Username)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Redirect(returnUrl ?? "/");
    }
}
