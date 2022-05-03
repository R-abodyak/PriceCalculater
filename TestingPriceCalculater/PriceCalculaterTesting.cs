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
        ICostService costService;
        Dictionary<Product, List<Cost>> productCostsDictonary;
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
            productCostsDictonary = new Dictionary<Product, List<Cost>>();
            var x = productCostsDictonary.Count;
        }
        [Theory]
        [InlineData(20, 15, 19.84)]
        public void TestFinalPrice1(decimal TaxPercentage, decimal DiscountPercentage, decimal expected)
        {
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            costService = new CostService(productCostsDictonary);
            calculater1 = new Calculater(MyTax, discountService, costService);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            Assert.Equal(expected, FinalPrice, 2);
        }
        [Theory]
        [InlineData(20, 15, "19.84\r\n4.46\r\n")]
        //[InlineData(21, 15, "21.46\r\n3.04\r\n")]
        public void TestReportwithCurrency(decimal TaxPercentage, decimal DiscountPercentage, String ExpectedOutput)
        {   product.Currency = Iso_3.GBP;
            MyTax = new TaxService(TaxPercentage);
            discountService = new DiscountService(DiscountPercentage, UpcDiscountDictonary);
            costService = new CostService(productCostsDictonary);

            calculater1 = new Calculater(MyTax, discountService, costService);
            var productPriceDetails = calculater1.FindProductDetails(product);
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            IDisplayService ConsoleDisplay = new ConsoleDisplayService();
            Report report = new Report(ConsoleDisplay);
            report.DisplayProductReport(productPriceDetails);
            Assert.Equal("Discount Amount : 4.46\r\n4.46 GBP\r\nFinal Price :19.84\r\n19.84 GBP\r\n", stringWriter.ToString());
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
            costService = new CostService(productCostsDictonary);
            calculater1 = new Calculater(MyTax, mockDiscount.Object, costService);
            decimal FinalPrice = calculater1.CalculateFinalPrice(product);
            ProductPriceDetails p = calculater1.FindProductDetails(product);
            Assert.Equal(19.78m, p.FinalPrice, 2);
            Assert.Equal(4.24m, p.DiscountAmount, 2);
        }
        [Fact]
        public void testExpensesBrancha()
        {
            Cost packging = new Cost()
            {
                Category = CostCategory.Pacakging,
                AmountType = CostAmountType.percentage,
                AmountValue = 1
            };
            Cost transport = new Cost
            {
                Category = CostCategory.Transport,
                AmountType = CostAmountType.relative,
                AmountValue = 2.2m
            };
            costService = new CostService(productCostsDictonary);
            costService.AddNewCost(product, packging);
            costService.AddNewCost(product, transport);
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            Calculater calculater2 = new Calculater(MyTax, discountService, costService);
            var productPriceDetails = calculater2.FindProductDetails(product);
            Assert.Equal(4.46m, productPriceDetails.DiscountAmount);
            Assert.Equal(4.25m, productPriceDetails.TaxAmount);
            Assert.Equal(0.2m, productPriceDetails.ProductCosts[0].CostCalculatedResult);
            Assert.Equal(2.2m, productPriceDetails.ProductCosts[1].CostCalculatedResult);
            Assert.Equal(2.40m, productPriceDetails.TotalCostAmount);
            Assert.Equal(22.44m, productPriceDetails.FinalPrice);
        }

        [Fact]
        public void TestCombiningOutputWithoutDisplay()
        {
            Cost packging = new Cost()
            {
                Category = CostCategory.Pacakging,
                AmountType = CostAmountType.percentage,
                AmountValue = 1
            };
            Cost transport = new Cost
            {
                Category = CostCategory.Transport,
                AmountType = CostAmountType.relative,
                AmountValue = 2.2m
            };
            costService = new CostService(productCostsDictonary);
            costService.AddNewCost(product, packging);
            costService.AddNewCost(product, transport);
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            Calculater calculater2 = new Calculater(MyTax, discountService, costService);
            var productPriceDetails = calculater2.FindProductDetails(product);
            Assert.Equal(4.46m, productPriceDetails.DiscountAmount);
            Assert.Equal(4.25m, productPriceDetails.TaxAmount);
            Assert.Equal(0.2m, productPriceDetails.ProductCosts[0].CostCalculatedResult);
            Assert.Equal(2.2m, productPriceDetails.ProductCosts[1].CostCalculatedResult);
            Assert.Equal(2.40m, productPriceDetails.TotalCostAmount);
            Assert.Equal(22.44m, productPriceDetails.FinalPrice);
        }

        [Theory]
        [InlineData(AmountType.percentage, 20, 4.25, 4.05, 20.45)]
        [InlineData(AmountType.relative, 4, 4.25, 4.00, 20.50)]
        [InlineData(AmountType.percentage, 30, 4.25, 4.46, 20.04)]
        public void TestCapCases(AmountType type, decimal val, decimal tax, decimal discount, decimal total)
        {
            MyTax = new TaxService(21);
            discountService = new DiscountService(15, UpcDiscountDictonary);
            Cap cap = new Cap()
            {
                _capType = type,
                _amountValue = val,
            };
            costService = new CostService(productCostsDictonary);

            Calculater calculater2 = new Calculater(MyTax, discountService, costService);
            calculater2.CombiningDiscount = Combining.additive;
            var productPriceDetails = calculater2.FindProductDetails(product, cap);
            Assert.Equal(discount, productPriceDetails.DiscountAmount);
            Assert.Equal(tax, productPriceDetails.TaxAmount);
            Assert.Equal(total, productPriceDetails.FinalPrice);
        }

    }

}