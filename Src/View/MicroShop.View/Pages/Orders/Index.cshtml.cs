using Common.Data;
using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrdersService _ordersService;

        public PagedResult<OrderDto[]>? Orders { get; set; }

        public IndexModel(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public async Task OnGet()
        {
            this.Orders = await _ordersService.GetOrdersAsync();
        }
    }
}
