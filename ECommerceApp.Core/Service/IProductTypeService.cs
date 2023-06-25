using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IProductTypeService : IGenericService<ProductType>
    {
        Task<List<ProductType>> GetProductTypes();
        Task<List<ProductType>> AddProductType(ProductType productType);
        Task<List<ProductType>> UpdateProductType(ProductType productType);
    }
}
