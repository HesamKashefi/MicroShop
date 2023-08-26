using MicroShop.View.Models.DTOs;
using MicroShop.View.Models.HttpClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroShop.View.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrdersService _ordersService;

        public DetailsModel(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public OrderDetailsDto OrderDetails { get; set; }

        [FromRoute]
        public int OrderId { get; init; }

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _ordersService.GetOrderAsync(OrderId);
            if (result.Status == Common.Data.DomainStatusCodes.OK)
            {
                OrderDetails = result.Data!;
                return Page();
            }
            else
            {
                return RedirectToPage("/Orders");
            }
        }
    }
}
