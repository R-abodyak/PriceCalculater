using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator
{
    public class Cap
    {
        public AmountType CapType { get; set; } = AmountType.Absolute;
        public decimal AmountValue { get; set; } = 0;
    }

    public enum AmountType
    {
        Percentage,
        Absolute
    }
}