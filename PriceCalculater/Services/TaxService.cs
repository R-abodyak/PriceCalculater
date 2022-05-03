namespace PriceCalculator.Services;

public class TaxService : ITaxService
{
    private readonly decimal _percentage;

    public TaxService(decimal percentage)
    {
        this._percentage = percentage;
    }

    public decimal GetTaxPercentage()
    {
        return _percentage;
    }
}