﻿using System.ComponentModel.DataAnnotations;

namespace Cart.Api.Models
{
    public record CartCheckout(
        [Required, StringLength(50, MinimumLength = 3)] string Country,
        [Required, StringLength(50, MinimumLength = 3)] string City,
        [Required, StringLength(50, MinimumLength = 3)] string Street,
        [Required, StringLength(50, MinimumLength = 3)] string ZipCode);
}
