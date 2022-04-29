using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PriceCalculater.Services;
    public  interface IDiscountService
    {
        public List<Discount> GetDiscountPercentage(Product product);
       public ECombining CombiningWay { get; set; }
}
    public class DiscountService : IDiscountService
   { 
        private readonly decimal _universalpercentage;
        private readonly Dictionary<long, decimal> _UpcDiscountList;
        private Precednce universalPrecedence= Precednce.after, upcPrecedence=Precednce.after;
       public ECombining CombiningWay { get; set; } = ECombining.additive;
    public DiscountService(decimal universalPercentage , Dictionary<long, decimal> UpcDiscountList)
        {
            this._universalpercentage = universalPercentage;
            this._UpcDiscountList = UpcDiscountList;
        }
    public DiscountService(decimal universalPercentage,Precednce universalPrecedence,
        Dictionary<long, decimal> UpcDiscountList ,Precednce upcPrecedence)
    {
        this._universalpercentage = universalPercentage;
        this._UpcDiscountList = UpcDiscountList;
        this.universalPrecedence = universalPrecedence;
        this.upcPrecedence = upcPrecedence; 
    }
    public List<Discount> GetDiscountPercentage(Product product) {
      var Discounts = new List<Discount>();
        if (_universalpercentage > 0) Discounts.Add
                (new Discount(_universalpercentage,DiscountType.universal,universalPrecedence));
        if (_UpcDiscountList.ContainsKey(product.UPC)) Discounts.Add
                (new Discount(_UpcDiscountList[product.UPC],DiscountType.upc,upcPrecedence));
         return Discounts;
    }
}
