using PriceCalculater;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
namespace TestingPriceCalculater
{
    public class TaxTest
    {   Product product;
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
        [InlineData(20,15,19.84)]
        public void TestFinalPrice1(decimal TaxPercentage,decimal DiscountPercentage ,decimal expected )
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage);
            UpcdiscountService = new UpcDiscountService(UpcDiscountDictonary,product.UPC);
            calculater1 = new Calculater(MyTax, discountService, UpcdiscountService);
            ProductPriceDetails productPriceDetails =calculater1.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            Assert.Equal(expected, FinalPrice,2);
        }
       
        [Theory]
        [InlineData(20, 15, "19.84\r\n4.46\r\n")]
        //[InlineData(21, 15, "21.46\r\n3.04\r\n")]
        public void TestReport(decimal TaxPercentage, decimal DiscountPercentage, String ExpectedOutput)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage);
            UpcdiscountService = new UpcDiscountService(UpcDiscountDictonary, product.UPC);
            calculater1 = new Calculater(MyTax, discountService, UpcdiscountService);
            ProductPriceDetails productPriceDetails = calculater1.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal(ExpectedOutput, stringWriter.ToString());
        }
    }
}