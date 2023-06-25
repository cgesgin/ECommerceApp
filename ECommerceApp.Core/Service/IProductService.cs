using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
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
        Task<Product> GetProductWithVariantAndType(int id);
        Task<List<Product>> GetProductsWithVariant();
        Task<ProductSearchResultDto> SearchProducts(string searchText,int page);
        Task<List<string>> GetProductsSearchSuggestions(string searchText);
        Task<List<Product>> GetFeaturedProducts();

        Task<List<Product>> GetAdminProducts();
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productId);
    }
}
