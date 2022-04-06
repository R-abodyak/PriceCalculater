﻿public class Report
{
   private readonly IDisplayService _displayService;
    private readonly ProductPriceDetails _productPriceDetails;
    public Report(IDisplayService displayService , ProductPriceDetails productPriceDetails)
    {
        this._displayService = displayService;
        this._productPriceDetails = productPriceDetails;
    }
    public void DisplayProductReport()
    {   _displayService.Display(_productPriceDetails.FinalPrice);
        _displayService.Display(_productPriceDetails.DiscountAmount+_productPriceDetails.UpcDiscountAmount);
        _displayService.Display(_productPriceDetails.TotalCostAmount);
    }

    }

