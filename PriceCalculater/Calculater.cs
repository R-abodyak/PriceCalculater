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
        price.ApplyPrecision();
        decimal amount = (percentage / 100) * price;
        return amount.ApplyPrecision();
    }
        private void FindProductDetails(Product product)
    {
        productPriceDetails = new ProductPriceDetails();
        decimal discountBeforeTax = CalculateDiscountBefore(product);
        decimal remaningPrice = product.Price - discountBeforeTax;
        productPriceDetails.TaxAmount = Calculate(remaningPrice, _taxService.GetTaxPercentage());
        decimal discountAfterTax = CalculateDiscountAfter(product, remaningPrice);
        productPriceDetails.DiscountAmount = discountBeforeTax + discountAfterTax;
        productPriceDetails.FinalPrice = product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount.ApplyPrecision();
    }
   
    public decimal CalculateFinalPrice(Product product) {
        FindProductDetails(product);
        return productPriceDetails.FinalPrice;
    }
    public decimal CalculateDiscountBefore(Product product) {
        List<Discount> discounts = GetDiscounts(product);
        decimal remainingPrice = product.Price;
        var result =discounts.Select(discounts=>discounts).
                     Where(d=>d.Prcedence==Precednce.before);
        decimal dicountBefore = 0;
        foreach(var item in result)
        {
            decimal discount  = Calculate(remainingPrice, item.Value);
            dicountBefore += discount;
            remainingPrice -= discount;
        }
        return dicountBefore;
    }
    public decimal CalculateDiscountAfter(Product product ,decimal price)
    {
        decimal discountAfter = 0;
        List<Discount> discounts = GetDiscounts(product);
        var result = discounts.Select(discounts => discounts).
                     Where(d => d.Prcedence == Precednce.after);
        foreach(var item in result)
        {
            discountAfter += Calculate(price, item.Value);
        }
        return discountAfter;
    }
    private List<Discount> GetDiscounts(Product product)
    {
        return _discountService.GetDiscountPercentage(product);
    }
}
