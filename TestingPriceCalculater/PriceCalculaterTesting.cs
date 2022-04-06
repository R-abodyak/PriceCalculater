using PriceCalculater;
using PriceCalculater.Cost;
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
            MyTax = new TaxService(20);
            discountService = new DiscountService(15);
            UpcdiscountService = new UpcDiscountService(UpcDiscountDictonary, product.UPC);
            calculater1 = new Calculater(MyTax, discountService, UpcdiscountService);
        }
      
        public void TestFinalPrice1( )
        {
           
            ProductPriceDetails productPriceDetails = calculater1.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            Assert.Equal(19.84m, FinalPrice, 2);
        }

        [Fact]

        public void TestReport( )
        {
           
            ProductPriceDetails productPriceDetails = calculater1.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal("19.84\r\n4.46\r\n", stringWriter.ToString());
        }
        [Fact]
        public void testPrecedenceBranch()
        {
            MyTax = new TaxService(20);
            discountService = new DiscountService(15);
            UpcdiscountService = new UpcDiscountService(UpcDiscountDictonary, product.UPC, true);
            calculater1 = new Calculater(MyTax, discountService, UpcdiscountService);
            ProductPriceDetails productPriceDetails = calculater1.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal("19.78\r\n4.24\r\n", stringWriter.ToString());
        }
        [Fact]
        public void testExpensesBrancha()
        {
            List<Cost> costList = new List<Cost>();
            Cost packging = new Cost(CostDescription.Pacakging, CostAmountType.percentage, 1);
            Cost transport = new Cost(CostDescription.Transport, CostAmountType.relative, 2.2m);
            costList.Add(packging);
            costList.Add(transport);
            Calculater calculater2 = new Calculater(MyTax, discountService, UpcdiscountService, costList);
            ProductPriceDetails productPriceDetails = calculater2.FindProductDetails(product.Price);
            decimal FinalPrice = productPriceDetails.FinalPrice;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal("22.44\r\n4.46\r\n2.40\r\n", stringWriter.ToString());
        }
    }
}