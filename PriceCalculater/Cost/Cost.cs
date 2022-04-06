namespace PriceCalculater.Cost
{
    public class  Cost
    {
        
        public  CostDescription Description { get; }
        public  AmountType AmountType { get; }
        public  decimal AmountValue { get; }
        public decimal Calculate (decimal price)
        {
            if (AmountType == AmountType.relative) return AmountValue;
            else return (AmountValue / 100) * price;
        }
       public Cost(CostDescription Description, AmountType AmountType, decimal AmountValue)
        {
            this.Description = Description;
            this.AmountType = AmountType;
            this.AmountValue = AmountValue;
        }
    }
}
