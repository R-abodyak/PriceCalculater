using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator
{
    public static class ExtensionClass
    {
        public static decimal ApplyPrecision(this decimal price, int precision)
        {
            return Math.Round(price, precision);
        }
    }
}