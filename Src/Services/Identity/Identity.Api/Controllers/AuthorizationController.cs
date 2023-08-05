using Identity.Api.Data;
using Identity.Api.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

[ApiController]
[AllowAnonymous]
public class AuthorizationController : ControllerBase
{
    private readonly IdentityContext _identityContext;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(
        IdentityContext identityContext,
        IOpenIddictScopeManager scopeManager,
        ILogger<AuthorizationController> logger)
    {
        _identityContext = identityContext;
        _scopeManager = scopeManager;
        _logger = logger;
    }

    [HttpPost("/connect/token")]
    public async Task<IActionResult> ExchangeToken()
    {
        var openIdRequest = HttpContext.GetOpenIddictServerRequest() ?? throw new Exception("OpenId Request cannot be null");

        if (openIdRequest.IsPasswordGrantType())
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Username == openIdRequest.Username);
            if (user is null)
            {
                _logger.LogTrace("User was not found in the database");

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = "Invalid Grant",
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "User unauthorized",
                }!);
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            if (!user.CheckPassword(openIdRequest.Password))
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = "Invalid Grant",
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "User unauthorized",
                }!);
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Create a new authentication ticket.
            var ticket = await CreateTicketAsync(user, openIdRequest);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
        else if (openIdRequest.IsAuthorizationCodeGrantType())
        {
            // Retrieve the claims principal stored in the authorization code.
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (!info.Succeeded)
            {
                return BadRequest(new OpenIddictResponse
                {
                    Error = Errors.InvalidGrant,
                    ErrorDescription = "UnAuthorized User"
                });
            }

            var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Username == info.Principal.Identity!.Name);

            if (user is null)
            {
                _logger.LogTrace("User was not found in the database");

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = "Invalid Grant",
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "User unauthorized",
                }!);
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            // Create a new authentication ticket.
            var ticket = await CreateTicketAsync(user, openIdRequest);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
        else
        {
            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }
    }



    [HttpGet("/connect/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var openIdRequest = HttpContext.GetOpenIddictServerRequest() ?? throw new Exception("OpenId Request cannot be null");

        var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!auth.Succeeded)
        {
            using var scope = _logger.BeginScope("User is not authenticated. Request: {@OpenIdRequest}", openIdRequest);

            if (openIdRequest.HasPrompt(Prompts.None))
            {
                _logger.LogTrace("Returning UnAuthorized. Prompt not set");

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = "Invalid Grant",
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "User unauthorized",
                }!);
                return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            _logger.LogTrace("Prompt value : {@Prompt}", openIdRequest.Prompt);
            return Challenge(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        var user = await _identityContext.Users.FirstOrDefaultAsync(x => x.Username == auth.Principal.Identity!.Name);

        if (user is null)
        {
            _logger.LogTrace("User was not found in the database");

            var properties = new AuthenticationProperties(new Dictionary<string, string>
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = "Invalid Grant",
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "User unauthorized",
            }!);
            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Create a new authentication ticket.
        var ticket = await CreateTicketAsync(user, openIdRequest);

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
    }


    private async Task<AuthenticationTicket> CreateTicketAsync(User user, OpenIddictRequest request, AuthenticationProperties? properties = null)
    {
        var claims = new List<Claim>()
        {
            new(Claims.Subject, user.Id.ToString()),
            new(Claims.Name, user.Username)
        };
        if (!string.IsNullOrEmpty(user.Roles))
        {
            claims.AddRange(user.Roles.Split(",").Select(x => new Claim(Claims.Role, x)));
        }
        var identity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
        var principal = new ClaimsPrincipal(identity);

        // Create a new authentication ticket holding the user identity.
        var ticket = new AuthenticationTicket(principal,
            properties ?? new AuthenticationProperties(),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // Set the list of scopes granted to the client application.
        // Note: the offline_access scope must be granted
        // to allow OpenIddict to return a refresh token.
        var availableScopes = new List<string>
            {
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Profile,
            Scopes.Roles
            };
        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            availableScopes.Add(Scopes.OfflineAccess);
        }

        // Set the list of scopes granted to the client application.
        var ticketScopes = principal.GetResources();
        var allScopes = request.GetScopes().Union(ticketScopes).ToImmutableArray();
        var allowedScopes = availableScopes.Intersect(allScopes).ToImmutableArray();

        // get resources
        var resources = new List<string>();
        await foreach (var item in _scopeManager.ListResourcesAsync(allScopes))
        {
            resources.Add(item);
        }

        principal.SetScopes(allowedScopes);
        principal.SetResources(resources);
        principal.SetDestinations(GetDestinations);

        return ticket;
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or
            Claims.Role or
            Claims.Subject
                // when claim.Subject.HasScope(Scopes.Profile)
                //_identityOptions.Value.ClaimsIdentity.SecurityStampClaimType
                => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            _ => new[] { Destinations.AccessToken },
        };
    }
}