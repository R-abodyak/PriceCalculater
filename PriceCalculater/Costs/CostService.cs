using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator.Costs
{
    public class CostService : ICostService
    {
        private readonly Dictionary<Product, List<Cost>> _productsCostsList;

        public CostService(Dictionary<Product, List<Cost>>? productsCostsList)
        {
            if (productsCostsList == null)
            {
                throw new ArgumentNullException("Products Costs List can't be null");
            }

            _productsCostsList = productsCostsList;
        }

        public List<Cost> GetCosts(Product product)
        {
            if (_productsCostsList.ContainsKey(product))
            {
                return _productsCostsList[product];
            }

            return null;
        }

        public void AddNewCost(Product product, Cost cost)

        {
            if (!_productsCostsList.ContainsKey(product))
            {
                _productsCostsList.Add(product, new List<Cost>() {cost});
            }
            else
            {
                _productsCostsList[product].Add(cost);
            }
        }

        void ICostService.RemoveCosts(Product product)
        {
            if (_productsCostsList.ContainsKey(product))
            {
                _productsCostsList.Remove(product);
            }
        }
    }
}