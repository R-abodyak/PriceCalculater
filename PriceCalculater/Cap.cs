using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PriceCalculater
{
    public class Cap
    {
        private EAmountType _capType;
        private decimal _amountValue;
        Cap(EAmountType capType, decimal amountValue)
        {
            this._capType = capType;
            this._amountValue = amountValue;
        }
    }
}
