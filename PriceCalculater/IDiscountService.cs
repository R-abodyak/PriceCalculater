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
    public class UpcDiscountService : IDiscountService
    {
        private readonly Dictionary<long,decimal> _UpcDiscountList;
        private readonly long _upc;
        public UpcDiscountService(Dictionary<long, decimal> UpcDiscountList,long upc)
        {
            this._UpcDiscountList = UpcDiscountList;
            this._upc = upc;
        }
        public decimal GetDiscountPercentage()
        {
            if (!_UpcDiscountList.ContainsKey(_upc)) return 0;
            return _UpcDiscountList[_upc];
        }
    }
}
