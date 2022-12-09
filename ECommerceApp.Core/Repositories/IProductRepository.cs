using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductByCategoryAsync(string categoryName);
        Task<Product> GetProductWithVariantAndType(int id);
        Task<List<Product>> GetProductsWithVariant();
        Task<List<Product>> SearchProducts(string searchText);
        Task<List<Product>> SearchProducts(string searchText,int page,float pageResult);
        Task<List<string>> GetProductsSearchSuggestions(string searchText);
        Task<List<Product>> GetFeaturedProducts();
    }
}
