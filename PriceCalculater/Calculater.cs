using PriceCalculater;
using PriceCalculater.Cost;
public class Calculater {
    private  readonly ITaxService _taxService;
    private readonly IDiscountService _discountService;
    private readonly IDiscountService _upcDiscountService;
    private readonly List<Cost> _costList;
    public Combining CombiningDiscount { get; set; }
    public Calculater(ITaxService taxService , IDiscountService discountService , 
        IDiscountService upcDiscountService,List<Cost> costList)
    {    if (taxService == null || discountService == null || upcDiscountService == null)
            throw new Exception("why are u passing me null :( ");
        _taxService = taxService;
        _discountService = discountService;
        _upcDiscountService = upcDiscountService;
        _costList = costList;
            }
    public Calculater(ITaxService taxService, IDiscountService discountService,
        IDiscountService upcDiscountService):this ( taxService,  discountService,
         upcDiscountService, null){ }
  public decimal Calculate(decimal price ,decimal percentage)
    {
        ApplyPrecision(price);
        decimal amount = (percentage / 100) * price;
        return ApplyPrecision(amount);
    }
    public decimal ApplyPrecision(decimal price)
    {
        return Math.Round(price, 2);
    }
     public ProductPriceDetails FindProductDetails(decimal price)
    {
        ProductPriceDetails productPriceDetails = new ProductPriceDetails();
        productPriceDetails.BasePrice = price;
        productPriceDetails.FinalPrice = price;
        decimal discountBefore= CalculateDiscountBefore(productPriceDetails);
        decimal discountAfter= CalculateDiscountAfter(productPriceDetails);
        decimal tax = productPriceDetails.TaxAmount = Calculate(productPriceDetails.FinalPrice, _taxService.GetTaxPercentage());
        decimal costAmount = 0;
        if (_costList!= null && _costList.Count != 0)
        {
            foreach(Cost item in _costList)
            {
                costAmount+= productPriceDetails.TotalCostAmount+= item.Calculate(price);
            }
        }
        productPriceDetails.FinalPrice = ApplyPrecision(productPriceDetails.FinalPrice- discountAfter+tax+costAmount);
        return productPriceDetails;
    }
    public decimal CalculateDiscountBefore( ProductPriceDetails productPriceDetails) {
        decimal dicountBefore = 0;
         void DecreseFinalPrice(decimal discount) 
        {productPriceDetails.FinalPrice -= discount; }
        if (_discountService.GetIsBefore())
        {
            decimal discount = productPriceDetails.DiscountAmount = Calculate(productPriceDetails.FinalPrice, _discountService.GetDiscountPercentage());
            DecreseFinalPrice(discount);
            dicountBefore += discount;
        }
        if (_upcDiscountService.GetIsBefore()) {
            decimal discount = productPriceDetails.UpcDiscountAmount = Calculate(productPriceDetails.FinalPrice, _upcDiscountService.GetDiscountPercentage());
            DecreseFinalPrice(discount);
            dicountBefore += discount;
        }
        return dicountBefore;
    }
    public decimal CalculateDiscountAfter(ProductPriceDetails productPriceDetails)
    {
        decimal discountAfter = 0;
        if (!_discountService.GetIsBefore())
        {
            discountAfter += productPriceDetails.DiscountAmount = Calculate(productPriceDetails.FinalPrice, _discountService.GetDiscountPercentage());
        }
        if (!_upcDiscountService.GetIsBefore())
        {
            discountAfter += productPriceDetails.UpcDiscountAmount = Calculate(productPriceDetails.FinalPrice, _upcDiscountService.GetDiscountPercentage());
        }
        return discountAfter;
    }
}
public enum Combining
{
    additive , multiplictive
}
