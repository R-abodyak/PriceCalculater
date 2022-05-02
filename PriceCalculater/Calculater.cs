using PriceCalculater.Costs;
using PriceCalculater.Services;
namespace PriceCalculater;
public class Calculater
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly ICostService _costService;

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
        price.ApplyPrecision();
        decimal amount = (percentage / 100) * price;
        return amount.ApplyPrecision();
    }
    public ProductPriceDetails FindProductDetails(Product product)
    {
        var productPriceDetails = new ProductPriceDetails();
        decimal discountBeforeTax = CalculateDiscountBefore(product);
        decimal remaningPrice = product.Price - discountBeforeTax;
        productPriceDetails.TaxAmount = Calculate(remaningPrice, _taxService.GetTaxPercentage());
        decimal discountAfterTax = CalculateDiscountAfter(product, remaningPrice);
        productPriceDetails.DiscountAmount = discountBeforeTax + discountAfterTax;
        var costList = _costService.GetCosts(product);
        FindCostDetails(productPriceDetails, product, costList);
        productPriceDetails.FinalPrice = (product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount + productPriceDetails.TotalCostAmount).ApplyPrecision();
        return productPriceDetails;
    }

    private void FindCostDetails(ProductPriceDetails productPriceDetails, Product product, List<Cost>? _costList)
    {
        decimal totalCostAmount = 0;
        decimal costAmount = 0;
        if (_costList != null && _costList.Count != 0)
        {
            foreach (Cost item in _costList)
            {
                if (item.AmountType == CostAmountType.relative) costAmount = item.AmountValue;
                else costAmount = Calculate(product.Price, item.AmountValue);
                totalCostAmount += costAmount;
                productPriceDetails.ProductCosts.Add((item.Category, costAmount));
            }
        }
        productPriceDetails.TotalCostAmount = totalCostAmount;

    }

    public decimal CalculateFinalPrice(Product product)
    {
        var productPriceDetails = FindProductDetails(product);
        return productPriceDetails.FinalPrice;
    }
    public decimal CalculateDiscountBefore(Product product)
    {
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        var dicountBefore = discounts.Where(d => d.Prcedence == Precednce.before).
            Sum(d => Calculate(product.Price, d.Value));

        return dicountBefore;
    }
    public decimal CalculateDiscountAfter(Product product, decimal price)
    {
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        var dicountAfter = discounts.Where(d => d.Prcedence == Precednce.after).
            Sum(d => Calculate(price, d.Value));

        return dicountAfter;
    }

}
