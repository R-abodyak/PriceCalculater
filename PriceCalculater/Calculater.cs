using PriceCalculater;

public class Calculater {
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly IDiscountService _upcDiscountService;

    public Calculater(ITaxService taxService , IDiscountService discountService , IDiscountService upcDiscountService)
    {    if (taxService == null || discountService == null || upcDiscountService == null)
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
        _upcDiscountService = upcDiscountService;

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
        productPriceDetails.UpcDiscountAmount = Calculate(price, _upcDiscountService.GetDiscountPercentage());
        productPriceDetails.FinalPrice = ApplyPrecision(price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount- productPriceDetails.UpcDiscountAmount);
        return productPriceDetails;
    }
}
