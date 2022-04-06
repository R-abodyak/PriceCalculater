
namespace PriceCalculater.Cost
{
    public class  Cost
    {
        private CostDescription pacakging;
        private CostAmountType percentage;
        private int v;

        public  CostDescription _description { get; }
        public  CostAmountType _amountType { get; }
        public  decimal _amountValue { get; }

        public decimal Calculate (decimal price)
        {
            if (_amountType == CostAmountType.relative) return _amountValue;
            else return (_amountValue / 100) * price;
        }
       public Cost(CostDescription Description, CostAmountType AmountType, decimal AmountValue)
        {
            this._description = Description;
            this._amountType = AmountType;
            this._amountValue = AmountValue;
        }

        
    }
}
