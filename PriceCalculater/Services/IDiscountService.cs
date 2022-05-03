
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PriceCalculater.Services;
    public  interface IDiscountService
    {
        public List<Discount> GetDiscountPercentage(Product product);
    }
