using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace ECommerceApp.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        public List<Product> Products { get ; set ; }
        public List<Product> AdminProducts { get; set; }
        public HttpClient _httpClient { get; set; }
        public string Message { get; set; } ="Ürünler Yükleniyor...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; }=String.Empty;

        public event Action ProductsChanged;
        public ProductService(HttpClient httpClient) 
        {
            _httpClient= httpClient;
            Products = new List<Product>();
        }

        public async Task GetAll(string ?category = null)
        {
            var result = category == null ? 
                await _httpClient.GetFromJsonAsync<ResponseDto<List<Product>>>("api/Product/featured") : 
                await _httpClient.GetFromJsonAsync<ResponseDto<List<Product>>>($"api/Product/category/{category}") ;
            if (result != null && result.Data!=null)
            {
                Products = result.Data;
            }

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
            {
                Message = "Ürün Bulunamadı";
            }
            ProductsChanged.Invoke();
        }

        public async Task<ResponseDto<Product>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<Product>>($"api/Product/{id}");
            return result;
        }

        public async Task SearchProducts(string searchText,int page)
        {
            LastSearchText = searchText;
            var result  = await _httpClient
                .GetFromJsonAsync<ResponseDto<ProductSearchResultDto>>($"api/Product/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount=result.Data.Pages;
            }
            if (Products.Count==0)
            {
                Message = "Ürün Bulunamadı.";
            }

           

            ProductsChanged?.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestion(string searchText)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<List<string>>>($"api/Product/searchsuggestions/{searchText}");
            return result.Data;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _httpClient.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ResponseDto<Product>>()).Data;
            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await _httpClient.DeleteAsync($"api/product/{product.Id}");
        }

        public async Task GetAdminProducts()
        {
            var result = await _httpClient
                .GetFromJsonAsync<ResponseDto<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/product", product);
            var content = await result.Content.ReadFromJsonAsync<ResponseDto<Product>>();
            return content.Data;
        }

    }
}
