using PriceCalculater;
using PriceCalculater.Costs;
public class ProductPriceDetails
{
    public decimal FinalPrice
    {
        get; set;
    }
    public decimal BasePrice
    {
        get; set;
    }
    public decimal TaxAmount
    {
        get; set;
    }
    public decimal DiscountAmount
    {
        get; set;
    }
    public decimal TotalCostAmount
    {
        get; set;
    }
    public Iso_3 Currency
    {
        get; set;
    }
    public List<(CostCategory CostDescription, decimal CostCalculatedResult)> ProductCosts;
    public ProductPriceDetails()
    {
        ProductCosts = new List<(CostCategory CostDescription, decimal CostCalculatedResult)>();
    }
}