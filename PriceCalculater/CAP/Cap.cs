using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculater.CAP
{
    public class Cap
    {
        private AmountType _capType;
        private decimal _amountValue;
        Cap(AmountType capType, decimal amountValue)
        {
            this._capType = capType;
            this._amountValue = amountValue;
        }
    }
}
