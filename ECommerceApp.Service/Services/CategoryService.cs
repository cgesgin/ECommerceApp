using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork , ICategoryRepository categoryRepository) : base(repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Category>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return await GetAdminCategories();
        }

        public async Task<List<Category>> DeleteCategory(int id)
        {
            Category category = await GetCategoryById(id);
            if (category == null)
            {
                return null;
            }

            category.Deleted = true;
            await _unitOfWork.CommitAsync();

            return await GetAdminCategories();
        }

        private async Task<Category> GetCategoryById(int id)
        {
            return await  _categoryRepository.Where(x=>x.Id==id).FirstAsync();
        }

        public async Task<List<Category>> GetAdminCategories()
        {
            var categories = await _categoryRepository.Where(c => !c.Deleted).ToListAsync();
            return categories;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _categoryRepository.Where(c => !c.Deleted && c.Visible).ToListAsync();
            return categories;
        }

        public async Task<List<Category>> UpdateCategory(Category category)
        {
            var dbCategory = await _categoryRepository.GetByIdAsync(category.Id);
            if (dbCategory == null)
            {
                return null;
            }

            dbCategory.Name = category.Name;
            dbCategory.Visible = category.Visible;

            await _unitOfWork.CommitAsync();

            return await GetAdminCategories();
        }
    }
}
