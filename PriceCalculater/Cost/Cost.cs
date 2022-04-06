namespace PriceCalculater.Cost
{
    public class  Cost
    {
        private CostDescription pacakging;
        private CostAmountType percentage;
        private int v;
        public  CostDescription Description { get; }
        public  CostAmountType AmountType { get; }
        public  decimal AmountValue { get; }
        public decimal Calculate (decimal price)
        {
            if (AmountType == CostAmountType.relative) return AmountValue;
            else return (AmountValue / 100) * price;
        }
       public Cost(CostDescription Description, CostAmountType AmountType, decimal AmountValue)
        {
            this.Description = Description;
            this.AmountType = AmountType;
            this.AmountValue = AmountValue;
        }
    }
}
