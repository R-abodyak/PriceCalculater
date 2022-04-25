using PriceCalculater;
public class Calculater {
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    public Calculater(ITaxService taxService , IDiscountService discountService )
    {    if (taxService == null || discountService == null )
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
    }
  public decimal Calculate(decimal price ,decimal percentage)
    {
        ApplyPrecision(price);
        decimal amount = (percentage / 100) * price;
        return ApplyPrecision(amount);
    }
    public decimal ApplyPrecision(decimal price)
    {
        return Math.Round(price, 2);
    }
     public ProductPriceDetails FindProductDetails(decimal price)
    {
        ProductPriceDetails productPriceDetails = new ProductPriceDetails();
        productPriceDetails.TaxAmount = Calculate(price, _taxService.GetTaxPercentage());
        productPriceDetails.DiscountAmount = Calculate(price, _discountService.GetDiscountPercentage());
        productPriceDetails.FinalPrice = ApplyPrecision(price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount- productPriceDetails.UpcDiscountAmount);
        return productPriceDetails;
    }
}
