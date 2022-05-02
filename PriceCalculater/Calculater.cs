using PriceCalculater.Services;
namespace PriceCalculater;
public class Calculater
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    public Calculater(ITaxService taxService, IDiscountService discountService)
    {
        if (taxService == null || discountService == null)
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
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
        productPriceDetails.FinalPrice = product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount.ApplyPrecision();
        return productPriceDetails;
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
