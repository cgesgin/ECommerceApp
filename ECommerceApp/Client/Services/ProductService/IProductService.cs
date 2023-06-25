using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;

namespace ECommerceApp.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        String Message { get; set; }
        List<Product> Products { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetAll(string? category = null);
        Task<ResponseDto<Product>> GetByIdAsync(int id);
        Task SearchProducts(string searchText , int page);
        Task <List<string>> GetProductSearchSuggestion(string searchText);

        Task GetAdminProducts();
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        List<Product> AdminProducts { get; set; }
    }
}
