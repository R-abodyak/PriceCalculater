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
        _displayService.Display("Discount Amount : ",_productPriceDetails.DiscountAmount);
        _displayService.Display("Cost : " ,_productPriceDetails.TotalCostAmount);
        _displayService.Display("Final Price :",_productPriceDetails.FinalPrice);
        DisplayCostItems();
    }
    private void DisplayCostItems()
    { if(_productPriceDetails.ProductCosts != null && _productPriceDetails.ProductCosts.Count != 0)
        {
            foreach( var item in _productPriceDetails.ProductCosts)
            {  
                _displayService.Display(item.CostDescription.ToString(), item.CostCalculatedResult);
            }
        }
    }
    }
