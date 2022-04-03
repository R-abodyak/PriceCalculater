public class ProductPriceDetails
{
    public decimal FinalPrice { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal UpcDiscountAmount { get; internal set; }
}