namespace PriceCalculater.Costs
{
    public class  Cost
    {
        public  CostDescription Description { get; }
        public  CostAmountType AmountType { get; }
        public  decimal AmountValue { get; }
       public Cost(CostDescription Description, CostAmountType AmountType, decimal AmountValue)
        {
            this.Description = Description;
            this.AmountType = AmountType;
            this.AmountValue = AmountValue;
        }
    }
}
