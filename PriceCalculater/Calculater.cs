public class Calculater {
   
 
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
    public ProductPriceDetails FindFinalPrice(decimal price, decimal Taxpercentage,decimal DiscountPercentage)
    {
        ProductPriceDetails productPriceDetails = new ProductPriceDetails();
         productPriceDetails.TaxAmount = Calculate(price, Taxpercentage);
        productPriceDetails.DiscountAmount = Calculate(price, DiscountPercentage);
        productPriceDetails.FinalPrice = ApplyPrecision(price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount);
        return productPriceDetails;
    }
}
