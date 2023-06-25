using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace ECommerceApp.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        public List<Category> Categories { get; set; }=new List<Category>();
        public List<Category> AdminCategories { get; set; } = new List<Category>();

        public event Action OnChange;

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

        public async Task AddCategory(Category category)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ResponseDto<List<Category>>>()).Data;
            await GetAll();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCategory = new Category { IsNew = true, Editing = true };
            AdminCategories.Add(newCategory);
            OnChange.Invoke();
            return newCategory;
        }

        public async Task DeleteCategory(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"api/Category/admin/{categoryId}");
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ResponseDto<List<Category>>>()).Data;
            await GetAll();
            OnChange.Invoke();
        }
        public async Task UpdateCategory(Category category)
        {
            var response = await _httpClient.PutAsJsonAsync("api/Category/admin", category);
            AdminCategories = (await response.Content
                .ReadFromJsonAsync<ResponseDto<List<Category>>>()).Data;
            await GetAll();
            OnChange.Invoke();
        }

        public async Task GetAdminCategories()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<Category>>>("api/Category/admin");
            if (response != null && response.Data != null)
                AdminCategories = response.Data;
        }
    }
}
