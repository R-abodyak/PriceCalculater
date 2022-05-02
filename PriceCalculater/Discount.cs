namespace PriceCalculater.Services
{
    public class Discount
    {
        public decimal Value { set; get; }

        public Discount(decimal value)
        {
            Value = value;
        }
    }
}