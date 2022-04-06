using PriceCalculater.Cost;
public class Report
{
   private readonly IDisplayService _displayService;
    private readonly ProductPriceDetails _productPriceDetails;
    private readonly List<Cost> _costList;
    public Report(IDisplayService displayService , ProductPriceDetails productPriceDetails, List<Cost> costList)
    {
        this._displayService = displayService;
        this._productPriceDetails = productPriceDetails;
        this._costList = costList;
    }
    public Report(IDisplayService displayService, ProductPriceDetails productPriceDetails) : this(displayService, productPriceDetails, null) { }
    public void DisplayProductReport()
    {
        _displayService.Display("Discount Amount : ",_productPriceDetails.DiscountAmount+_productPriceDetails.UpcDiscountAmount);
        _displayService.Display("Cost : " ,_productPriceDetails.TotalCostAmount);
        _displayService.Display("Final Price :",_productPriceDetails.FinalPrice);
        DisplayCostItems();
    }
    private void DisplayCostItems()
    { if(_costList!=null && _costList.Count != 0)
        {
            foreach( var item in _costList)
            {  
                _displayService.Display(item.Description.ToString(), item.Calculate(_productPriceDetails.BasePrice));
            }
        }
    }
    }
