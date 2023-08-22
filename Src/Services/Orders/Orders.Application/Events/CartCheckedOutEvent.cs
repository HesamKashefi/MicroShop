using EventBus.Core;
using Orders.Application.Models;

namespace Orders.Application.Events
{
    public class UserCheckoutStartedEvent : Event
    {
        public UserCheckoutStartedEvent(int buyerId, Cart cart, string country, string city, string street, string zipCode)
        {
            BuyerId = buyerId;
            Cart = cart ?? throw new ArgumentNullException(nameof(cart));
            Country = country ?? throw new ArgumentNullException(nameof(country));
            City = city ?? throw new ArgumentNullException(nameof(city));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        }

        public int BuyerId { get; private set; }

        public Cart Cart { get; private set; }

        public string Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
    }
}
