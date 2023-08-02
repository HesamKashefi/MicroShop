using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MicroShop.View.Models
{
    public class AuthHttpClientHandler : HttpClientHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHttpClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if(token is not null)
                    await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                throw;
            }
        }
    }
}
