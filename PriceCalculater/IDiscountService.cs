namespace PriceCalculater.Services;
    public  interface IDiscountService
    {
        public List<Discount> GetDiscountPercentage(Product product);
    }
