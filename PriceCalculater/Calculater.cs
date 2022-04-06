using PriceCalculater;
using PriceCalculater.CAP;
using PriceCalculater.Cost;
public class Calculater
{
    private readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly IDiscountService _upcDiscountService;
    private readonly List<Cost> _costList;
    public Combining CombiningDiscount { get; set; } = Combining.additive;
    public Cap Cap1 { get; set ; } 
    public Calculater(ITaxService taxService, IDiscountService discountService,
        IDiscountService upcDiscountService, List<Cost> costList)
    {
        if (taxService == null || discountService == null || upcDiscountService == null)
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
        _upcDiscountService = upcDiscountService;
        _costList = costList;

    }
    public Calculater(ITaxService taxService, IDiscountService discountService,
        IDiscountService upcDiscountService) : this(taxService, discountService,
         upcDiscountService, null)
    { }
    public decimal Calculate(decimal price, decimal percentage)
    {
        ApplyPrecision(price ,4);
        decimal amount = (percentage / 100) * price;
        return ApplyPrecision(amount,4);
    }
    public decimal ApplyPrecision(decimal price ,int i)
    {
        return Math.Round(price, i);
    }
    public ProductPriceDetails FindProductDetails(decimal price)
    {
        int internalPrecision = 4;
        int FinalPrecision = 2;
        ProductPriceDetails productPriceDetails = new ProductPriceDetails();
        productPriceDetails.BasePrice = ApplyPrecision(price, internalPrecision);
        productPriceDetails.FinalPrice = ApplyPrecision(price, internalPrecision);
        CalculateTax(productPriceDetails.FinalPrice, productPriceDetails); 
        CalculatCost(price, productPriceDetails);
        CalculateTotalDiscount(price, productPriceDetails);
        productPriceDetails.FinalPrice = ApplyPrecision(productPriceDetails.BasePrice - productPriceDetails.DiscountAmount - productPriceDetails.UpcDiscountAmount + productPriceDetails.TaxAmount + productPriceDetails.TotalCostAmount,FinalPrecision);
        return productPriceDetails;
    }
    private void CalculateTax(decimal price ,ProductPriceDetails productPriceDetails)
    {
        productPriceDetails.TaxAmount = Calculate(price, _taxService.GetTaxPercentage());
    }
    private void CalculatCost(decimal price, ProductPriceDetails productPriceDetails)
    {
        decimal costAmount = 0;
        if (_costList != null && _costList.Count != 0)
        {
            foreach (Cost item in _costList)
            {
                costAmount += productPriceDetails.TotalCostAmount += item.Calculate(price);
            }
        }
    }
    private decimal CalculateDiscountBefore(ProductPriceDetails productPriceDetails)
    {
        decimal dicountBefore = 0;
        void DecreseFinalPrice(decimal discount)
        { 
            productPriceDetails.FinalPrice -= discount; 
        }
        if (_discountService.GetIsBefore())
        {
            decimal discount = productPriceDetails.DiscountAmount = Calculate(productPriceDetails.BasePrice, _discountService.GetDiscountPercentage());
            DecreseFinalPrice(discount);
            dicountBefore += discount;
        }
        decimal CombiningPrice =GetCombiningPrice(productPriceDetails.BasePrice, productPriceDetails.FinalPrice);
        if (_upcDiscountService.GetIsBefore())
        {
            decimal discount = productPriceDetails.UpcDiscountAmount = Calculate(CombiningPrice, _upcDiscountService.GetDiscountPercentage());
            DecreseFinalPrice(discount);
            dicountBefore += discount;
        }
        return dicountBefore;
    }
    private decimal GetCombiningPrice(decimal price1,decimal price2)
    {
        decimal CombiningPrice = CombiningDiscount == Combining.additive ? price1 : price2;
        return CombiningPrice;
    }
    private decimal CalculateDiscountAfter(ProductPriceDetails productPriceDetails)
    {
        decimal discountAfter = 0;
        if (!_discountService.GetIsBefore())
        {
            discountAfter += productPriceDetails.DiscountAmount += Calculate(productPriceDetails.FinalPrice, _discountService.GetDiscountPercentage());
        }
        decimal partalFinalPrice = productPriceDetails.FinalPrice - discountAfter;
        decimal CombiningPrice = GetCombiningPrice(productPriceDetails.FinalPrice,partalFinalPrice);
        if (!_upcDiscountService.GetIsBefore())
        {
            discountAfter += productPriceDetails.UpcDiscountAmount = Calculate(CombiningPrice, _upcDiscountService.GetDiscountPercentage());
        }
        return discountAfter;
    }
    public decimal CalculateTotalDiscount(decimal basePrice,ProductPriceDetails productPriceDetails)
    {
        decimal discountBefore = CalculateDiscountBefore(productPriceDetails);
        decimal discountAfter = CalculateDiscountAfter(productPriceDetails);
        decimal TotalDiscountAmount = discountBefore + discountAfter;
        if (Cap1._amountValue == 0) return productPriceDetails.TotalDiscountAmount;
        decimal CapCalculatedResult = 0;
        if (Cap1._capType == AmountType.relative) CapCalculatedResult = Cap1._amountValue;
        else CapCalculatedResult = Calculate(basePrice, Cap1._amountValue);
        productPriceDetails.TotalDiscountAmount =Math.Min(TotalDiscountAmount, CapCalculatedResult);
        return productPriceDetails.TotalDiscountAmount;
    }
}
public enum Combining
{
    additive, multiplictive
}
