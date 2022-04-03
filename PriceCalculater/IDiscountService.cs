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
        public bool IsBefore;
        public DiscountService(decimal percentage) : this(percentage, false) { }
        public DiscountService(decimal percentage, bool IsBefore)
        {
            this.IsBefore = IsBefore;
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
        public bool IsBefore;
        public UpcDiscountService(Dictionary<long, decimal> UpcDiscountList,long upc)
        {
            this._UpcDiscountList = UpcDiscountList;
            this._upc = upc;
            IsBefore = false;
        }
        public UpcDiscountService(Dictionary<long, decimal> UpcDiscountList, long upc,bool IsBefore)
        {
            this._UpcDiscountList = UpcDiscountList;
            this._upc = upc;
            this.IsBefore = IsBefore;
        }
        public decimal GetDiscountPercentage()
        {
            if (!_UpcDiscountList.ContainsKey(_upc)) return 0;
            return _UpcDiscountList[_upc];
        }
    }
}
