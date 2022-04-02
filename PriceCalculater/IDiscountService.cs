using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculater
{
    public   interface IDiscountService
    {
        public decimal GetDiscountPercentage();
    }
    public class DiscountService : IDiscountService
    {
        private readonly decimal _percentage;
        public DiscountService(decimal percentage)
        {
            this._percentage = percentage;
        }
        public decimal GetDiscountPercentage()
        {
            return _percentage;
        }
    }
}
