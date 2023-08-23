using System.ComponentModel.DataAnnotations;

namespace MicroShop.View.Models.DTOs
{
    public record CartCheckoutDto(
        [Required, StringLength(50, MinimumLength = 3)] string Country,
        [Required, StringLength(50, MinimumLength = 3)] string City,
        [Required, StringLength(50, MinimumLength = 3)] string Street,
        [Required, StringLength(50, MinimumLength = 3)] string ZipCode);
}
