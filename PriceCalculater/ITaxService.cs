interface ITaxService
{
    public decimal GetTaxPercentage();
}
public class TaxService :ITaxService
{
    public decimal Percentage { get; set; } = 20;
    public decimal GetTaxPercentage()
    {
        return Percentage;
    }
}