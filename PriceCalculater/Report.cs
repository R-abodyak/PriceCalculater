using PriceCalculater.Costs;
public class Report
{
   private readonly IDisplayService _displayService;
    private readonly ProductPriceDetails _productPriceDetails;
    public Report(IDisplayService displayService , ProductPriceDetails productPriceDetails)
    {
        this._displayService = displayService;
        this._productPriceDetails = productPriceDetails;
    }
    public void DisplayProductReport()
    {
        _displayService.Display("Discount Amount : ",_productPriceDetails.DiscountAmount, _productPriceDetails.Currency);
        _displayService.Display("Cost : " ,_productPriceDetails.TotalCostAmount, _productPriceDetails.Currency);
        _displayService.Display("Final Price :",_productPriceDetails.FinalPrice, _productPriceDetails.Currency);
        DisplayCostItems();
    }
    private void DisplayCostItems()
    { if(_productPriceDetails.ProductCosts != null && _productPriceDetails.ProductCosts.Count != 0)
        {
            foreach( var item in _productPriceDetails.ProductCosts)
            {  
                _displayService.Display(item.CostDescription.ToString(), item.CostCalculatedResult, _productPriceDetails.Currency);
            }
        }
    }
    }
