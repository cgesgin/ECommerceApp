using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;

namespace ECommerceApp.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetAll();
        Task<ResponseDto<Category>> GetByIdAsync(int id);
        event Action OnChange;
        List<Category> AdminCategories { get; set; }
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
        Category CreateNewCategory();
    }
}
