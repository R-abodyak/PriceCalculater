
namespace PriceCalculater.Cost
{
      class  Cost
    {
        private readonly CostDescription _description;
        private readonly CostAmountType _amountType;
        private readonly decimal _amountValue;
        Cost(CostDescription Description, CostAmountType AmountType, decimal AmountValue)
        {
            this._description = Description;
            this._amountType = AmountType;
            this._amountValue = AmountValue;
        }
    }
}
enum CostDescription {Transport,Pacakging, administrative }
enum CostAmountType { percentage, relative }