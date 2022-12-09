using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using ECommerceApp.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class BasketService : IBasketService 
    {
        protected readonly AppDbContext _appDbContext;

        public BasketService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public  async Task<List<BasketProductDto>> GetBasketProducts(List<BasketItemDto> basketItems)
        {
            var result =new List<BasketProductDto>();

            foreach (var item in basketItems)
            {
                var product = await _appDbContext.Products.Where(x=>x.Id==item.ProductId).FirstOrDefaultAsync();
                
                if (product == null)
                    continue;
                
                var productVarriant = await _appDbContext.ProductVariants.Where(x=>x.ProductId== item.ProductId
                && x.ProductTypeId== item.ProductTypeId).Include(x=>x.ProductType).FirstOrDefaultAsync();
                
                if (productVarriant == null)
                    continue;

                var basketProduct = new BasketProductDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVarriant.Price,
                    ProductType = productVarriant.ProductType.Name,
                    ProductTypeId = productVarriant.ProductTypeId,
                    Quantity=item.Quantity,
                    
                };
                result.Add(basketProduct);
            }
            return result;
        }
    }
}
