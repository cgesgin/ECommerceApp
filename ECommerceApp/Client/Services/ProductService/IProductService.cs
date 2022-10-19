using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;

namespace ECommerceApp.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task GetAll(string? category = null);
        Task<ResponseDto<Product>> GetByIdAsync(int id);
    }
}
