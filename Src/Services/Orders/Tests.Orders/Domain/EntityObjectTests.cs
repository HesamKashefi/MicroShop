using Orders.Domain;

namespace Tests.Orders.Domain
{
    public class EntityObjectTests
    {
        [Fact]
        public void Entities_With_Same_Id_Must_Be_Equal()
        {
            var orderA = new Order
            {
                Id = 1,
                Address = new Address
                {
                    Country = "Iran",
                    City = "Tehran",
                    Street = "Azadi",
                    ZipCode = "1158800000"
                },
                BuyerId = 1,
            };
            var orderB = new Order
            {
                Id = 1,
                Address = new Address
                {
                    Country = "USA",
                    City = "NewYork City",
                    Street = "Times",
                    ZipCode = "123456"
                },
                BuyerId = 1,
            };

            var equality = orderA == orderB;

            Assert.True(equality);
        }

        [Fact]
        public void Entities_With_Different_Id_Must_Not_Be_Equal()
        {
            var orderA = new Order
            {
                Id = 1,
                Address = new Address
                {
                    Country = "Iran",
                    City = "Tehran",
                    Street = "Azadi",
                    ZipCode = "1158800000"
                },
                BuyerId = 1,
            };
            var orderB = new Order
            {
                Id = 2,
                Address = new Address
                {
                    Country = "Iran",
                    City = "Tehran",
                    Street = "Azadi",
                    ZipCode = "1158800000"
                },
                BuyerId = 1,
            };

            var equality = orderA == orderB;

            Assert.False(equality);
        }
    }
}
