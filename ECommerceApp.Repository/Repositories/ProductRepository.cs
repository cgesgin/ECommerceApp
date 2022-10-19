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

        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
           return await _appDbContext.Products
                .Where(x => x.Category.Name.ToLower().Equals(categoryName.ToLower()))
                .ToListAsync();
        }
    }
}
