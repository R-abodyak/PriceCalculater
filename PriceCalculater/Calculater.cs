using PriceCalculater.Services;
namespace PriceCalculater;
public class Calculater {
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
   public ProductPriceDetails productPriceDetails { get; set; }
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
     private void FindProductDetails(Product product)
    {
         productPriceDetails = new ProductPriceDetails();
        productPriceDetails.TaxAmount = Calculate(product.Price, _taxService.GetTaxPercentage());
        List<Discount>  discounts = _discountService.GetDiscountPercentage(product);
        foreach(Discount discount in discounts) { 
            productPriceDetails.DiscountAmount+=Calculate(product.Price, discount.Value);
        }
        productPriceDetails.FinalPrice = ApplyPrecision(product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount);
        
    }
    public decimal CalculatePrice(Product product) {
        FindProductDetails(product);
        return productPriceDetails.FinalPrice;
    }
}
