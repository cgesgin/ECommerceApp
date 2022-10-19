﻿using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<Product>> GetProductByCategoryAsync(string categoryName);
    }
}
