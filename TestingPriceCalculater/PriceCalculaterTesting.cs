using PriceCalculater;
using PriceCalculater.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
namespace TestingPriceCalculater
{
    public class TaxTest
    {
        Product product;
        Calculator calculater1;
        IDiscountService discountService;
        Dictionary<long, decimal> UpcDiscountDictonary;
        ITaxService MyTax;
        public TaxTest()
        {
            product = new Product
            {
                Name = "mybook",
                UPC = 1234,
                Price = 20.25M
            };

            UpcDiscountDictonary = new Dictionary<long, decimal>
            {
                {1234,7 },
                {567,12 }
            };
        }
        [Theory]
        [InlineData(20, 15, 19.84)]
        public void TestFinalPrice1(decimal TaxPercentage, decimal DiscountPercentage, decimal expected)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            calculater1 = new Calculator(MyTax, discountService);
            decimal FinalPrice = calculater1.CalculatePrice(product);
            Assert.Equal(expected, FinalPrice, 2);
        }

        [Theory]
        [InlineData(20, 15, "19.84\r\n4.46\r\n")]
        //[InlineData(21, 15, "21.46\r\n3.04\r\n")]
        public void TestReport(decimal TaxPercentage, decimal DiscountPercentage, String ExpectedOutput)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            calculater1 = new Calculator(MyTax, discountService);
            ProductPriceDetails productPriceDetails = calculater1.FindProductDetails(product);
            decimal FinalPrice = calculater1.CalculatePrice(product);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay);
            report.DisplayProductReport(productPriceDetails);
            Assert.Equal(ExpectedOutput, stringWriter.ToString());
        }
    }
}