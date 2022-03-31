using Xunit;
namespace TestingPriceCalculater
{
    public class TaxTest
    {
        
       
        [Fact]
        public void TestFinalPrice1()
        {
            Product product;
            Calculater calculater1;

            product = new Product
            {
                Name = "mybook",
                UPC = 123,
                Price = 20.25M
            };
            calculater1 = new Calculater();
            ITaxService MyTax = new TaxService(20);
            decimal FinalPrice = calculater1.FindFinalPrice(product.Price, MyTax.GetTaxPercentage());
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            ConsoleDisplay.Display(product.Price);
            ConsoleDisplay.Display(FinalPrice);
            Assert.Equal(24.30M, FinalPrice,2);
        }
    
    }
}