namespace PriceCalculater.Services
{
    public class Discount
    {
        public decimal Value { set; get; } 
        public DiscountType Type { get; set; }
        public Discount(decimal value,DiscountType type) {
            Value = value;
            Type = type;
        }
    }
}
public enum DiscountType { universal , upc}