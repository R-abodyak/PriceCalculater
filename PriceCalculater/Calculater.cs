using PriceCalculater.Costs;
﻿using PriceCalculater.Services;
namespace PriceCalculater;
public class Calculater {
    private  readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    public ProductPriceDetails productPriceDetails { get; set; }
    public Calculater(ITaxService taxService , IDiscountService discountService )
    {    if (taxService == null || discountService == null )
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
    }
    public decimal Calculate(decimal price ,decimal percentage)
    {
        price.ApplyPrecision();
        decimal amount = (percentage / 100) * price;
        return amount.ApplyPrecision();
    }
        private void FindProductDetails(Product product, List<Cost>? _costList =null)
    {
        productPriceDetails = new ProductPriceDetails();
        decimal discountBeforeTax = CalculateDiscountBefore(product);
        decimal remaningPrice = product.Price - discountBeforeTax;
        productPriceDetails.TaxAmount = Calculate(remaningPrice, _taxService.GetTaxPercentage());
        decimal discountAfterTax = CalculateDiscountAfter(product, remaningPrice);
        productPriceDetails.DiscountAmount = discountBeforeTax + discountAfterTax;
        FindCostDetails(product, _costList);
        productPriceDetails.FinalPrice = (product.Price + productPriceDetails.TaxAmount - productPriceDetails.DiscountAmount + productPriceDetails.TotalCostAmount).ApplyPrecision();
    }
    private void FindCostDetails(Product product, List<Cost>? _costList)
    {
        decimal totalCostAmount = 0;
        decimal costAmount = 0;
        if (_costList != null && _costList.Count != 0)
        {
            foreach (Cost item in _costList)
            {
                if (item.AmountType == CostAmountType.relative) costAmount = item.AmountValue;
                else costAmount = Calculate(product.Price, item.AmountValue);
                totalCostAmount += costAmount;
                productPriceDetails.ProductCosts.Add((item.Description, costAmount));
            }
        }
        productPriceDetails.TotalCostAmount = totalCostAmount;
    }
    public decimal CalculateFinalPrice(Product product, List<Cost>? _costList = null) 
    {
        FindProductDetails(product, _costList);
        return productPriceDetails.FinalPrice;
     }
    public decimal CalculateDiscountBefore(Product product) {
        List<Discount> discounts = GetDiscounts(product);
        decimal Price  = product.Price;
        decimal remainingPrice = product.Price;
        var result =discounts.Select(discounts=>discounts).
                     Where(d=>d.Prcedence==Precednce.before);
        decimal dicountBefore = 0;
        foreach(var item in result)
        {
            decimal discount  = Calculate(Price, item.Value);
            dicountBefore += discount;
            remainingPrice -= discount;
            if (_discountService.CombiningWay == ECombining.multiplictive) Price = remainingPrice;
        }
        return dicountBefore;
    }
    public decimal CalculateDiscountAfter(Product product ,decimal price)
    {
        decimal discountAfter = 0;
        decimal discount = 0;
        decimal remainingPrice = price;
        List<Discount> discounts = GetDiscounts(product);
        var result = discounts.Select(discounts => discounts).
                     Where(d => d.Prcedence == Precednce.after);
        foreach(var item in result)
        {   discount = Calculate(price, item.Value);
            discountAfter += discount;
            remainingPrice-= discount;
            if (_discountService.CombiningWay == ECombining.multiplictive) price = remainingPrice;
        }
        return discountAfter;
    }
    private List<Discount> GetDiscounts(Product product)
    {
        return _discountService.GetDiscountPercentage(product);
    }
}
