using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        public List<Category> Categories { get; set; }=new List<Category>();

        public HttpClient _httpClient { get; set; }

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Categories = new List<Category>();
        }

        public async Task GetAll()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<List<Category>>>("api/Category");
            if (result != null && result.Data != null)
            {
                Categories = result.Data;
            }
        }

        public async Task<ResponseDto<Category>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<Category>>($"api/Category/{id}");
            return result;
        }
    }
}
