using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PriceCalculater
{
    public class Cap
    {
        public EAmountType _capType { get; } = EAmountType.relative;
        public decimal _amountValue { get; } = 0;
        public Cap(EAmountType capType, decimal amountValue)
        {
            this._capType = capType;
            this._amountValue = amountValue;
        }
    }
}
