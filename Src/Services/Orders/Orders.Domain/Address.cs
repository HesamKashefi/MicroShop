namespace Orders.Domain
{
    public class Address : ValueObject<Address>
    {
        public required string Country { get; init; }
        public required string City { get; init; }
        public required string Street { get; init; }
        public required string ZipCode { get; init; }
    }
}