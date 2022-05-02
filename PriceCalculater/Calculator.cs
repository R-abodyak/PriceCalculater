using PriceCalculater.Services;

namespace PriceCalculater;

public class Calculator
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;

    public Calculator(ITaxService taxService, IDiscountService discountService)
    {
        if (taxService == null)
            throw new Exception("Tax Services can't be null ");

        if (discountService == null)
            throw new Exception("Discount Services can't be null");
        _taxService = taxService;
        _discountService = discountService;
    }

    public decimal Calculate(decimal price, decimal percentage)
    {
        decimal amount = (percentage / 100) * ApplyPrecision(price);
        return ApplyPrecision(amount);
    }

    public decimal ApplyPrecision(decimal price)
    {
        return Math.Round(price, 2);
    }

    public ProductPriceDetails FindProductDetails(Product product)
    {
        ProductPriceDetails productPriceDetails = new ProductPriceDetails();
        productPriceDetails.TaxAmount = Calculate(product.Price, _taxService.GetTaxPercentage());
        List<Discount> discounts = _discountService.GetDiscountPercentage(product);
        foreach (Discount discount in discounts)
        {
            productPriceDetails.DiscountAmount += Calculate(product.Price, discount.Value);
        }

        productPriceDetails.FinalPrice =
            ApplyPrecision(product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount);
        return productPriceDetails;
    }

    public decimal CalculatePrice(Product product)
    {
        var productPriceDetails = FindProductDetails(product);
        return productPriceDetails.FinalPrice;
    }
}