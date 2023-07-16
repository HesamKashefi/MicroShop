using System.ComponentModel.DataAnnotations;

namespace Cart.Api.Models
{
    public class Cart : IValidatableObject
    {
        public List<CartItem> CartItems { get; init; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var vr = new List<ValidationResult>();
            if (validationContext.ObjectInstance is Cart cart)
            {
                var ids = cart.CartItems.Select(x => x.ProductId);
                if (ids.Count() != ids.Distinct().Count())
                {
                    vr.Add(new ValidationResult("Duplicated Item in the cart is not allowed"));
                }
            }
            return vr;
        }
    }
}
