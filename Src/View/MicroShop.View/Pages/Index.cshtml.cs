using Common.Data;
using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICatalogService _catalogService;

        [FromQuery]
        public int PageNumber { get; set; } = 1;

        public PagedResult<ProductDto[]>? Products { get; set; }


        public IndexModel(ILogger<IndexModel> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                this.Products = await _catalogService.GetProductsAsync(PageNumber);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get products");
            }
        }
    }
}