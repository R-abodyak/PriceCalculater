using Moq;
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
        Calculater calculater1;
        IDiscountService discountService;
        IDiscountService UpcdiscountService;
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
            calculater1 = new Calculater(MyTax, discountService);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            Assert.Equal(expected, FinalPrice,2);
        }
        [Theory]
        [InlineData(20, 15, "19.84\r\n4.46\r\n")]
        //[InlineData(21, 15, "21.46\r\n3.04\r\n")]
        public void TestReport(decimal TaxPercentage, decimal DiscountPercentage, String ExpectedOutput)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            calculater1 = new Calculater(MyTax, discountService);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, calculater1.productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal(ExpectedOutput, stringWriter.ToString());
        }
        [Fact]
        public void testPrecedenceBranch()
        {
            MyTax = new TaxService(20);
            discountService = new DiscountService(15,Precednce.after,
                UpcDiscountDictonary,Precednce.before);
            Mock<IDiscountService> mockDiscount = new Mock<IDiscountService>();
            mockDiscount.Setup(x => x.GetDiscountPercentage(product)).Returns(
                new List<Discount>() {
                    new Discount(15,DiscountType.universal,Precednce.after),
                    new Discount(7,DiscountType.upc,Precednce.before),
                });
            calculater1 = new Calculater(MyTax, mockDiscount.Object);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, calculater1.productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal("19.78\r\n4.24\r\n", stringWriter.ToString());
        }
    }
}