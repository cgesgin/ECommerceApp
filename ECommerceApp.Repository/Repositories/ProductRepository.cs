using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Repository.Data;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductRepository(AppDbContext appDbContext, IHttpContextAccessor contextAccessor) : base(appDbContext)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<List<Product>> GetFeaturedProducts()
        {
            return await _appDbContext.Products
                 .Where(p => p.Featured && p.Visible && !p.Deleted)
                 .Include(p => p.ProductVariants.Where(v => v.Visible && !v.Deleted))
                 .Include(p => p.Images)
                 .ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
           return await _appDbContext.Products
                .Include(v => v.ProductVariants)
                .Where(x => x.Category.Name.ToLower().Equals(categoryName.ToLower()))
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsForAdmin()
        {
            return await _appDbContext.Products.Where(x => !x.Deleted).Include(x => x.ProductVariants.Where(x => !x.Deleted)).ThenInclude(x => x.ProductType).Include(p => p.Images).ToListAsync();
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
                 .Include(p => p.Images)
                 .ToListAsync();
        }

        public async Task<Product> GetProductWithVariantAndType(int id)
        {
            Product product = null;
            
            if (_contextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _appDbContext.Products
                    .Include(p => p.ProductVariants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                      .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);
            }
            else
            {
                product = await _appDbContext.Products
                    .Include(p => p.ProductVariants.Where(v => v.Visible && !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                      .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && p.Visible);
            }
            return product;
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
            ).Include(v => v.ProductVariants).Include(p => p.Images).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var dbProduct = await _appDbContext.Products
              .Include(p => p.Images)
              .FirstOrDefaultAsync(p => p.Id == product.Id);
            if (dbProduct == null)
            {
                return null;
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            var productImages = dbProduct.Images;
            _appDbContext.Images.RemoveRange(productImages);

            dbProduct.Images = product.Images;

            foreach (var variant in product.ProductVariants)
            {
                var dbVariant = await _appDbContext.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId &&
                        v.ProductTypeId == variant.ProductTypeId);
                if (dbVariant == null)
                {
                    variant.ProductType = null;
                    _appDbContext.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _appDbContext.SaveChangesAsync();
            return product;
        }

        private async Task<List<Product>> FindProductsBySearchText(string searchText)
        {
            return await _appDbContext.Products.Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                    p.Description.ToLower().Contains(searchText.ToLower()) &&
                                    p.Visible && !p.Deleted).Include(v => v.ProductVariants).ToListAsync();
        }
    }
}
