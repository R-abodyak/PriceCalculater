namespace PriceCalculater.Costs
{
    public class Cost
    {
        public CostCategory Category
        {
            get; set;
        }
        public CostAmountType AmountType
        {
            get; set;
        }
        public decimal AmountValue
        {
            get; set;
        }

    }
}
