using Moq;
using PriceCalculater;
using PriceCalculater.Costs;
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
            Assert.Equal(expected, FinalPrice, 2);
        }
        [Theory]
        [InlineData(20, 15, "19.84\r\n4.46\r\n")]
        //[InlineData(21, 15, "21.46\r\n3.04\r\n")]
        public void TestReportWithCurrency(decimal TaxPercentage, decimal DiscountPercentage, String ExpectedOutput)
        {
            product.Currency = Iso_3.GBP;
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            calculater1 = new Calculater(MyTax, discountService);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay, calculater1.productPriceDetails);
            report.DisplayProductReport();
            Assert.Equal("Discount Amount : 4.46\r\n4.46  GBP\r\nFinal Price :19.84\r\n19.84  GBP\r\n", stringWriter.ToString());
        }
        [Fact]
        public void testPrecedenceBranch()
        {
            MyTax = new TaxService(20);
            discountService = new DiscountService(15, Precednce.after,
                UpcDiscountDictonary, Precednce.before);
            Mock<IDiscountService> mockDiscount = new Mock<IDiscountService>();
            mockDiscount.Setup(x => x.GetDiscountPercentage(product)).Returns(
                new List<Discount>() {
                    new Discount(15,DiscountType.universal,Precednce.after),
                    new Discount(7,DiscountType.upc,Precednce.before),
                });
            calculater1 = new Calculater(MyTax, mockDiscount.Object);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            ProductPriceDetails p = calculater1.productPriceDetails;
            Assert.Equal(19.78m, p.FinalPrice,2);
            Assert.Equal(4.24m, p.DiscountAmount, 2);
        }
        [Fact]
        public void testExpensesBrancha()
        {
            List<Cost> costList = new List<Cost>();
            Cost packging = new Cost(CostDescription.Pacakging, CostAmountType.percentage, 1);
            Cost transport = new Cost(CostDescription.Transport, CostAmountType.relative, 2.2m);
            costList.Add(packging);
            costList.Add(transport);
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            Calculater calculater2 = new Calculater(MyTax, discountService);
             calculater2.CalculateFinalPrice(product,costList);
            Assert.Equal(4.46m, calculater2.productPriceDetails.DiscountAmount);
            Assert.Equal(4.25m, calculater2.productPriceDetails.TaxAmount);
            Assert.Equal(0.2m, calculater2.productPriceDetails.ProductCosts[0].CostCalculatedResult);
            Assert.Equal(2.2m, calculater2.productPriceDetails.ProductCosts[1].CostCalculatedResult);
            Assert.Equal(2.40m, calculater2.productPriceDetails.TotalCostAmount);
            Assert.Equal(22.44m, calculater2.productPriceDetails.FinalPrice);
        }
        [Fact]
        public void TestCombiningOutputWithoutDisplay()
        {
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            discountService.CombiningWay = ECombining.multiplictive;
            List<Cost> costList = new List<Cost>();
            Cost packging = new Cost(CostDescription.Pacakging, CostAmountType.percentage, 1);
            Cost transport = new Cost(CostDescription.Transport, CostAmountType.relative, 2.2m);
            costList.Add(packging);
            costList.Add(transport);
            Calculater calculater2 = new Calculater(MyTax, discountService);
            calculater2.CalculateFinalPrice(product, costList);
            Assert.Equal(4.24m, calculater2.productPriceDetails.DiscountAmount);
            Assert.Equal(4.25m, calculater2.productPriceDetails.TaxAmount);
            Assert.Equal(0.2m, calculater2.productPriceDetails.ProductCosts[0].CostCalculatedResult);
            Assert.Equal(2.2m, calculater2.productPriceDetails.ProductCosts[1].CostCalculatedResult);
            Assert.Equal(2.40m, calculater2.productPriceDetails.TotalCostAmount);
            Assert.Equal(22.66m, calculater2.productPriceDetails.FinalPrice);
        }
        [Theory]
        [InlineData(EAmountType.percentage, 20,4.25,4.05,20.45)]
        [InlineData(EAmountType.relative, 4,4.25,4.00,20.50)]
        [InlineData(EAmountType.percentage, 30, 4.25, 4.46, 20.04)]
        public void TestCapCases(EAmountType type , decimal val ,decimal tax, decimal discount, decimal total )
        {
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            discountService.CombiningWay = ECombining.additive;
            Cap cap = new Cap(type, val);
            Calculater calculater2 = new Calculater(MyTax, discountService);
            calculater2.CalculateFinalPrice(product,null,cap);
            Assert.Equal(discount, calculater2.productPriceDetails.DiscountAmount);
            Assert.Equal(tax, calculater2.productPriceDetails.TaxAmount);
             Assert.Equal(total, calculater2.productPriceDetails.FinalPrice);
        }


    }
}