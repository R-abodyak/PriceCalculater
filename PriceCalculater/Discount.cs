namespace PriceCalculater.Services
{
    public class Discount
    {
        public decimal Value { set; get; } 
        public DiscountType Type { get; set; }
        public Precednce Prcedence { get; set; }
        public Discount(decimal value,DiscountType type,Precednce precednce =Precednce.after) {
            Value = value;
            Type = type;
            Prcedence = precednce;
        }
    }
}
public enum DiscountType { universal , upc}
public enum Precednce { before, after}