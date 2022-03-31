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
    public decimal FindFinalPrice(decimal price, decimal Taxpercentage)
    {
        decimal tax = Calculate(price, Taxpercentage);
        return ApplyPrecision(price + tax);
    }
}
