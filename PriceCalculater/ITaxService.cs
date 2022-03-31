public interface ITaxService
{

    public decimal GetTaxPercentage();
}
public class TaxService :ITaxService
{
    private decimal _percentage;
    TaxService(decimal percentage)
    {
        this._percentage = percentage;
    }
    public decimal GetTaxPercentage()
    {
        return _percentage;
    }
}