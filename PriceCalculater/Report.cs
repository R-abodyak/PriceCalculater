public class Report
{
    private readonly IDisplayService _displayService;
    public Report(IDisplayService displayService)
    {
        this._displayService = displayService;
    }
    public void DisplayProductReport(ProductPriceDetails productPriceDetails)
    {
        _displayService.Display(productPriceDetails.FinalPrice);
        _displayService.Display(productPriceDetails.DiscountAmount);
    }
}
