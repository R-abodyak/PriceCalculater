using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculater.Costs
{
    public interface ICostService
    {
        List<Cost> GetCosts(Product product);
        void AddNewCost(Product product, Cost cost);
        void RemoveCosts(Product product);
    }
}
