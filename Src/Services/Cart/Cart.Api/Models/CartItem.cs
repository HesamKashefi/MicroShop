using System.ComponentModel.DataAnnotations;

namespace Cart.Api.Models
{
    public record CartItem(string ProductId, string ProductName, string ImageUrl, decimal ProductPrice, int Quantity) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vr = new List<ValidationResult>();
            if (validationContext.ObjectInstance is CartItem cartItem)
            {
                if (cartItem.Quantity < 1 || string.IsNullOrEmpty(ProductId))
                {
                    vr.Add(new ValidationResult("Invalid Item Quantity. Minimum quantity is 1.", new[] { nameof(Quantity) }));
                }
            }
            return vr;
        }
    }
}
