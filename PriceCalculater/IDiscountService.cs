using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PriceCalculater.Services;
    public  interface IDiscountService
    {
        public List<Discount> GetDiscountPercentage(Product product);
    }
    public class DiscountService : IDiscountService
   { 
        private readonly decimal _universalpercentage;
        private readonly Dictionary<long, decimal> _UpcDiscountList;
      public DiscountService(decimal universalPercentage , Dictionary<long, decimal> UpcDiscountList)
        {
            this._universalpercentage = universalPercentage;
            this._UpcDiscountList = UpcDiscountList;
        }
    public List<Discount> GetDiscountPercentage(Product product) {
      var Discounts = new List<Discount>();
        if (_universalpercentage > 0) Discounts.Add
                (new Discount(_universalpercentage,DiscountType.universal));
        if (_UpcDiscountList.ContainsKey(product.UPC)) Discounts.Add
                (new Discount(_UpcDiscountList[product.UPC],DiscountType.upc));
         return Discounts;
    }
    }
