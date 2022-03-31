using System;
using System.IO;
using Xunit;
namespace TestingPriceCalculater
{

    public class TaxTest
    {   Product product;
        Calculater calculater1;
        ITaxService MyTax;
        public TaxTest()
        {
            product = new Product
            {
                Name = "mybook",
                UPC = 123,
                Price = 20.25M
            };
            calculater1 = new Calculater();
        }
        [Theory]
        [InlineData(20,24.30)]
        [InlineData(21, 24.50)]
        public void TestFinalPrice1(int percentage ,decimal expected )
        {
            MyTax = new TaxService(percentage);
            decimal FinalPrice = calculater1.FindFinalPrice(product.Price, MyTax.GetTaxPercentage());
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Assert.Equal(expected, FinalPrice,2);
        }
        [Theory]
        [InlineData(20, "24.30\r\n")]
        [InlineData(21, "24.50\r\n")]
        public void TestDisplay(decimal percentage ,String ExpectedOutput)
        {
            MyTax = new TaxService(percentage);
            decimal FinalPrice = calculater1.FindFinalPrice(product.Price, MyTax.GetTaxPercentage());
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            ConsoleDisplay.Display(FinalPrice);
            Assert.Equal(ExpectedOutput, stringWriter.ToString());
        }
    }
}