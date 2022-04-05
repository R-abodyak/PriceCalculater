namespace PriceCalculater
{
    public class DiscountService : IDiscountService
    {
        private readonly decimal _percentage;
        private bool IsBefore;
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

        public bool GetIsBefore()
        {
            return IsBefore;
        }
    }
}
