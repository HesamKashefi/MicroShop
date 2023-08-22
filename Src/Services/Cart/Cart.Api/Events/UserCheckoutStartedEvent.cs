using EventBus.Core;

namespace Cart.Api.Events
{
    public class UserCheckoutStartedEvent : Event
    {
        public UserCheckoutStartedEvent(int buyerId, Models.Cart cart, string country, string city, string street, string zipCode)
        {
            BuyerId = buyerId;
            Cart = cart ?? throw new ArgumentNullException(nameof(cart));
            Country = country ?? throw new ArgumentNullException(nameof(country));
            City = city ?? throw new ArgumentNullException(nameof(city));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }

        public int BuyerId { get; }

        public Models.Cart Cart { get; }

        public string Country { get; }
        public string City { get; }
        public string Street { get; }
        public string ZipCode { get; }
    }
}
