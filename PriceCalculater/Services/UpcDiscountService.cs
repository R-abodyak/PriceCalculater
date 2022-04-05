namespace PriceCalculater
{
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
        public bool GetIsBefore()
        {
            return IsBefore;
        }
    }
}
