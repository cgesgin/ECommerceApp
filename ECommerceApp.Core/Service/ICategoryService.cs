using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<List<Category>> GetCategories();
        Task<List<Category>> GetAdminCategories();
        Task<List<Category>> AddCategory(Category category);
        Task<List<Category>> UpdateCategory(Category category);
        Task<List<Category>> DeleteCategory(int id);
    }
}
