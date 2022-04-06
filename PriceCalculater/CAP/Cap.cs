using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculater.CAP
{
    public class Cap
    {
        public AmountType _capType { get; }= AmountType.relative;
        public decimal _amountValue { get; } = 0;
        public Cap(AmountType capType, decimal amountValue)
        {
            this._capType = capType;
            this._amountValue = amountValue;
        }
    }
}
