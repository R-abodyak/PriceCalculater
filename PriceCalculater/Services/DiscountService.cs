namespace PriceCalculator.Services;

public class DiscountService : IDiscountService
{
    private readonly decimal _universalPercentage;
    private readonly Dictionary<long, decimal> _upcDiscountList;
    private readonly Precednce _universalPrecedence = Precednce.After;
    private readonly Precednce _upcPrecedence = Precednce.After;

    public DiscountService(decimal universalPercentage, Dictionary<long, decimal> upcDiscountList)

    {
        this._universalPercentage = universalPercentage;
        this._upcDiscountList = upcDiscountList;
    }

    public DiscountService(decimal universalPercentage, Precednce universalPrecedence,
        Dictionary<long, decimal> upcDiscountList, Precednce upcPrecedence)
    {
        this._universalPercentage = universalPercentage;
        this._upcDiscountList = upcDiscountList;
        this._universalPrecedence = universalPrecedence;
        this._upcPrecedence = upcPrecedence;
    }

    public List<Discount> GetDiscountPercentage(Product product)
    {
        var discounts = new List<Discount>();
        if (_universalPercentage > 0)
            discounts.Add
                (new Discount(_universalPercentage, DiscountType.Universal, _universalPrecedence));
        if (_upcDiscountList.ContainsKey(product.UPC))
            discounts.Add
                (new Discount(_upcDiscountList[product.UPC], DiscountType.Upc, _upcPrecedence));
        return discounts;
    }
}