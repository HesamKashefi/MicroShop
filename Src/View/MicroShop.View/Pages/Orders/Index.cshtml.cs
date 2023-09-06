using Common.Data;
using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrdersService _ordersService;

        [FromQuery]
        public int PageNumber { get; set; } = 1;

        public PagedResult<OrderDto[]>? Orders { get; set; }

        public IndexModel(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public async Task OnGet()
        {
            this.Orders = await _ordersService.GetOrdersAsync(PageNumber);
        }
    }
}
