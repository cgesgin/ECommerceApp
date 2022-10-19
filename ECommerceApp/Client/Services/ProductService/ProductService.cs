using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        public List<Product> Products { get ; set ; }
        public HttpClient _httpClient { get; set; }
        public event Action ProductsChanged;
        public ProductService(HttpClient httpClient) 
        {
            _httpClient= httpClient;
            Products = new List<Product>();
        }

        public async Task GetAll(string ?category = null)
        {
            var result = category == null? 
                await _httpClient.GetFromJsonAsync<ResponseDto<List<Product>>>("api/Product") : 
                await _httpClient.GetFromJsonAsync<ResponseDto<List<Product>>>($"api/Product/Category/{category}") ;
            if (result != null && result.Data!=null)
            {
                Products = result.Data;
            }
            ProductsChanged.Invoke();
        }

        public async Task<ResponseDto<Product>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<Product>>($"api/Product/{id}");
            return result;
        }
    }
}
