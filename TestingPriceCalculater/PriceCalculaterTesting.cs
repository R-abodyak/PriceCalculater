using PriceCalculater;
using System;
using System.IO;
using Xunit;
namespace TestingPriceCalculater
{
    public class TaxTest
    {   Product product;
        Calculater calculater1;
        IDiscountService discountService;
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
        [InlineData(20,15,21.26)]
        [InlineData(21,15, 21.46)]
        public void TestFinalPrice1(decimal TaxPercentage,decimal DiscountPercentage ,decimal expected )
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage);
            decimal FinalPrice = calculater1.FindFinalPrice(product.Price, MyTax.GetTaxPercentage(),discountService.GetDiscountPercentage());
            Assert.Equal(expected, FinalPrice,2);
        }
        [Theory]
        [InlineData(20,15, "21.26\r\n")]
        [InlineData(21,15, "21.46\r\n")]
        public void TestDisplay(decimal TaxPercentage , decimal DiscountPercentage,String ExpectedOutput)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage);
            decimal FinalPrice = calculater1.FindFinalPrice(product.Price, MyTax.GetTaxPercentage(), discountService.GetDiscountPercentage());
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            ConsoleDisplay.Display(FinalPrice);
            Assert.Equal(ExpectedOutput, stringWriter.ToString());
        }
    }
}