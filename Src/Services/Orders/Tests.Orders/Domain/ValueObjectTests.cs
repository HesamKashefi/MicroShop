using Orders.Domain;

namespace Tests.Orders.Domain
{
    public class ValueObjectTests
    {
        [Fact]
        public void Equality_Must_Return_True_For_Equal_Objects()
        {
            var address1 = new Address
            {
                Country = "Iran",
                City = "Tehran",
                Street = "Azadi",
                ZipCode = "1158800000"
            };

            var address2 = new Address
            {
                Country = "Iran",
                City = "Tehran",
                Street = "Azadi",
                ZipCode = "1158800000"
            };

            var isEqual = address1 == address2;

            Assert.True(isEqual);
        }


        [Fact]
        public void Equality_Must_Return_False_For_Not_Equal_Objects()
        {
            var address1 = new Address
            {
                Country = "Iran",
                City = "Tehran",
                Street = "Azadi",
                ZipCode = "1158800000"
            };

            var address2 = new Address
            {
                Country = "Iran",
                City = "Tehran",
                Street = "Azadi",
                ZipCode = "1158811111"
            };

            var isEqual = address1 == address2;

            Assert.False(isEqual);
        }
    }
}