namespace PriceCalculator.Services
{
    public class Discount
    {
        public decimal Value { set; get; }
        public DiscountType Type { get; set; }
        public Precednce Prcedence { get; set; }

        public Discount(decimal value, DiscountType type, Precednce precednce = Precednce.After)
        {
            Value = value;
            Type = type;
            Prcedence = precednce;
        }
    }
}

public enum DiscountType
{
    Universal,
    Upc
}

public enum Precednce
{
    Before,
    After
}