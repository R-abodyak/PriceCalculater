using PriceCalculater.Costs;
using PriceCalculater.Services;
namespace PriceCalculater;
public class Calculater
{
    delegate decimal ApplyCombining(decimal x);
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly ICostService _costService;
    public Combining CombiningDiscount { get; set; } = Combining.additive;

    public Calculater(ITaxService taxService, IDiscountService discountService, ICostService costService)
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
    public decimal Calculate(decimal price, decimal percentage)
    {
        decimal amount = (percentage / 100) * price.ApplyPrecision(4);
        return amount.ApplyPrecision(2);
    }
    public ProductPriceDetails FindProductDetails(Product product, Cap cap = null)
    {
        var productPriceDetails = new ProductPriceDetails();
        var discountBeforeTax = CalculateDiscountBefore(product);
        decimal remaningPrice = product.Price - discountBeforeTax;
        productPriceDetails.TaxAmount = Calculate(remaningPrice, _taxService.GetTaxPercentage());
        var discountAfterTax = CalculateDiscountAfter(product, remaningPrice);

        decimal totalDiscount = discountBeforeTax + discountAfterTax;
        decimal CapCalculatedResult = totalDiscount;
        CapCalculatedResult = CalculateCap(product, cap, CapCalculatedResult);
        productPriceDetails.DiscountAmount = Math.Min(totalDiscount, CapCalculatedResult);
        var costList = _costService.GetCosts(product);
        FindCostDetails(productPriceDetails, product, costList);
        productPriceDetails.FinalPrice = (product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount + productPriceDetails.TotalCostAmount).ApplyPrecision(2);
        productPriceDetails.Currency = product.Currency;
        return productPriceDetails;
    }
    private decimal CalculateCap(Product product, Cap cap, decimal CapCalculatedResult)
    {
        if (cap != null)
        {
            if (cap._capType == AmountType.relative) CapCalculatedResult = cap._amountValue;
            else CapCalculatedResult = Calculate(product.Price, cap._amountValue);
        }
        return CapCalculatedResult;
    }

    private void FindCostDetails(ProductPriceDetails productPriceDetails, Product product, List<Cost>? _costList)
    {
        decimal totalCostAmount = 0;
        decimal costAmount = 0;
        if (_costList != null && _costList.Count != 0)
        {
            foreach (Cost item in _costList)
            {
                if (item.AmountType == CostAmountType.Relative)
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
        decimal Price = product.Price;
        decimal remainingPrice = product.Price;
        var result = discounts.Select(discounts => discounts).
                     Where(d => d.Prcedence == Precednce.before);
        decimal dicountBefore = 0;
        foreach (var item in result)
        {
            decimal discount = Calculate(Price, item.Value);
            dicountBefore += discount;
            remainingPrice -= discount;
            if (CombiningDiscount == Combining.multiplictive) Price = remainingPrice;
        }
        return dicountBefore;
    }
    public decimal CalculateDiscountAfter(Product product, decimal price)
    {
        decimal discountAfter = 0;
        decimal discount = 0;
        decimal remainingPrice = price;
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        var result = discounts.Select(discounts => discounts).
                     Where(d => d.Prcedence == Precednce.after);
        foreach (var item in result)
        {
            discount = Calculate(price, item.Value);
            discountAfter += discount;
            remainingPrice -= discount;
            if (CombiningDiscount == Combining.multiplictive) price = remainingPrice;
        }
        return discountAfter;
    }


}


