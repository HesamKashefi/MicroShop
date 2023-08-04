using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICatalogService _catalogService;

        public ProductDto[]? Products { get; set; }


        public IndexModel(ILogger<IndexModel> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                this.Products = await _catalogService.GetProductsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get products");
            }
        }
    }
}