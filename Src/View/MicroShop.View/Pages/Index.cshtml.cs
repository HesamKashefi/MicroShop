using MicroShop.View.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _http;

        public ProductDto[]? Products { get; set; }


        public IndexModel(ILogger<IndexModel> logger, HttpClient http)
        {
            _logger = logger;
            _http = http;
        }

        public async Task OnGetAsync()
        {
            try
            {
                this.Products = await _http.GetFromJsonAsync<ProductDto[]>("products");
                _logger.LogTrace("Received Products {@Products}", new { this.Products });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get products");
            }
        }
    }
}