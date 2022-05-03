using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculater
{
    public class Cap
    {
        public AmountType _capType { get; set; } = AmountType.relative;
        public decimal _amountValue { get; set; } = 0;
    }
    public enum AmountType
    {
        percentage, relative
    }
}