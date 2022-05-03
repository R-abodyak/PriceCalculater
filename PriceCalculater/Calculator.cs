using PriceCalculator;
using PriceCalculator.Costs;
using PriceCalculator.Services;

namespace PriceCalculater;

public class Calculator
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly ICostService _costService;
    public Combining CombiningDiscount { get; set; } = Combining.Additive;

    public Calculator(ITaxService taxService, IDiscountService discountService, ICostService costService)
    {
        if (taxService == null)
            throw new Exception("Tax services can't be null");
        if (discountService == null)
            throw new Exception("Discount services can't be null");
        if (costService == null)
            throw new Exception("Cost services can't be null");
        _taxService = taxService;
        _discountService = discountService;
        _costService = costService;
    }

    private decimal Calculate(decimal price, decimal percentage)
    {
        decimal amount = (percentage / 100) * price.ApplyPrecision(4);
        return amount.ApplyPrecision(2);
    }

    public ProductPriceDetails FindProductDetails(Product product, Cap cap = null)
    {
        var productPriceDetails = new ProductPriceDetails();
        var discountBeforeTax = CalculateDiscountBefore(product);
        decimal remainingPrice = product.Price - discountBeforeTax;
        productPriceDetails.TaxAmount = Calculate(remainingPrice, _taxService.GetTaxPercentage());
        var discountAfterTax = CalculateDiscountAfter(product, remainingPrice);

        decimal totalDiscount = discountBeforeTax + discountAfterTax;
        decimal capCalculatedResult = totalDiscount;
        capCalculatedResult = CalculateCap(product, cap, capCalculatedResult);
        productPriceDetails.DiscountAmount = Math.Min(totalDiscount, capCalculatedResult);
        var costList = _costService.GetCosts(product);
        FindCostDetails(productPriceDetails, product, costList);
        productPriceDetails.FinalPrice =
            (product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount +
             productPriceDetails.TotalCostAmount).ApplyPrecision(2);
        productPriceDetails.Currency = product.Currency;
        return productPriceDetails;
    }

    private decimal CalculateCap(Product product, Cap cap, decimal capCalculatedResult)
    {
        if (cap != null)
        {
            if (cap.CapType == AmountType.Absolute) capCalculatedResult = cap.AmountValue;
            else capCalculatedResult = Calculate(product.Price, cap.AmountValue);
        }

        return capCalculatedResult;
    }

    private void FindCostDetails(ProductPriceDetails productPriceDetails, Product product, List<Cost>? _costList)
    {
        decimal totalCostAmount = 0;
        if (_costList != null && _costList.Count != 0)
        {
            foreach (Cost item in _costList)
            {
                decimal costAmount = 0;
                if (item.AmountType == CostAmountType.Absolute)
                    costAmount = item.AmountValue;
                else
                    costAmount = Calculate(product.Price, item.AmountValue);

                totalCostAmount += costAmount;
                productPriceDetails.ProductCosts.Add((item.Category, costAmount));
            }

            productPriceDetails.TotalCostAmount = totalCostAmount;
        }
    }

    public decimal CalculateFinalPrice(Product product)
    {
        var productPriceDetails = FindProductDetails(product);
        return productPriceDetails.FinalPrice;
    }

    public decimal CalculateDiscountBefore(Product product)
    {
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        decimal price = product.Price;
        decimal remainingPrice = product.Price;
        var result = discounts.Select(discounts => discounts).Where(d => d.Prcedence == Precednce.Before);
        decimal dicountBefore = 0;
        foreach (var item in result)
        {
            decimal discount = Calculate(price, item.Value);
            dicountBefore += discount;
            remainingPrice -= discount;
            if (CombiningDiscount == Combining.Multiplictive) price = remainingPrice;
        }

        return dicountBefore;
    }

    public decimal CalculateDiscountAfter(Product product, decimal price)
    {
        decimal discountAfter = 0;
        decimal discount = 0;
        decimal remainingPrice = price;
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        var result = discounts.Select(d => d).Where(d => d.Prcedence == Precednce.After);
        foreach (var item in result)
        {
            discount = Calculate(price, item.Value);
            discountAfter += discount;
            remainingPrice -= discount;
            if (CombiningDiscount == Combining.Multiplictive) price = remainingPrice;
        }

        return discountAfter;
    }
}