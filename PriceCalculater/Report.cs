using PriceCalculater.Costs;
public class Report
{
    private readonly IDisplayService _displayService;
    public Report(IDisplayService displayService)
    {
        this._displayService = displayService;

    }
    public void DisplayProductReport(ProductPriceDetails productPriceDetails)
    {
        _displayService.Display("Discount Amount : ", productPriceDetails.DiscountAmount, productPriceDetails.Currency);
        _displayService.Display("Cost : ", productPriceDetails.TotalCostAmount, productPriceDetails.Currency);
        _displayService.Display("Final Price :", productPriceDetails.FinalPrice, productPriceDetails.Currency);
        DisplayCostItems(productPriceDetails);
    }
    private void DisplayCostItems(ProductPriceDetails productPriceDetails)
    {
        if (productPriceDetails.ProductCosts != null && productPriceDetails.ProductCosts.Count != 0)
        {
            foreach (var item in productPriceDetails.ProductCosts)
            {
                _displayService.Display(item.CostDescription.ToString(), item.CostCalculatedResult, productPriceDetails.Currency);
            }
        }
    }
}
