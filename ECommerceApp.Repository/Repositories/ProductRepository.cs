using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Repository.Repositories
{
    public class ProductRepository :  GenericRepository<Product> , IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }

        public async Task<List<Product>> GetFeaturedProducts()
        {
            return await _appDbContext.Products
                .Where(x=>x.Featured).Include(x=>x.ProductVariants).ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
           return await _appDbContext.Products
                .Include(v => v.ProductVariants)
                .Where(x => x.Category.Name.ToLower().Equals(categoryName.ToLower()))
                .ToListAsync();
        }

        public async Task<List<string>> GetProductsSearchSuggestions(string searchText)
        {
            var products= await FindProductsBySearchText(searchText);
            List<string> suggestions = new List<string>();
            foreach (var item in products)
            {
                if (item.Title.Contains(searchText,StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(item.Title);
                }
                //desciripton in search
            }
            return suggestions;
        }

        public async Task<List<Product>> GetProductsWithVariant()
        {
            return await _appDbContext.Products
                 .Include(v => v.ProductVariants)
                 .ToListAsync();
        }

        public async Task<Product> GetProductWithVariantAndType(int id)
        {
            return await _appDbContext.Products
                .Include(v => v.ProductVariants)
                .ThenInclude(t => t.ProductType)
                .Where(x=>x.Id==id)
                .FirstAsync();
        }

        public async Task<List<Product>> SearchProducts(string searchText)
        {
            List<Product> result = await FindProductsBySearchText(searchText);
            return result;
        }

        public async Task<List<Product>> SearchProducts(string searchText, int page, float pageResult)
        {
            return await _appDbContext.Products.Where(x => x.Title.ToLower().Contains(searchText.ToLower())
            || x.Description.ToLower().Contains(searchText.ToLower())
            ).Include(v => v.ProductVariants).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _appDbContext.Products.Where(x => x.Title.ToLower().Contains(searchText.ToLower())
            || x.Description.ToLower().Contains(searchText.ToLower())
            ).Include(v => v.ProductVariants).ToListAsync();
        }
    }
}
