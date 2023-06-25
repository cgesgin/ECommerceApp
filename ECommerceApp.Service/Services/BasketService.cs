using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using ECommerceApp.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class BasketService : IBasketService
    {
        protected readonly AppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(AppDbContext appDbContext,IUnitOfWork unitOfWork)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddToBasket(BasketItem basketItem, int userId)
        {
            basketItem.UserId = userId;

            var sameItem = await _appDbContext.BasketItems
                .FirstOrDefaultAsync(ci => ci.ProductId == basketItem.ProductId &&
                ci.ProductTypeId == basketItem.ProductTypeId && ci.UserId == basketItem.UserId);
            if (sameItem == null)
            {
                _appDbContext.BasketItems.Add(basketItem);
            }
            else
            {
                sameItem.Quantity += basketItem.Quantity;
            }

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<int> GetBasketItems(int userId)
        {
            int count = (await _appDbContext.BasketItems.Where(x=>x.UserId==userId).ToListAsync()).Count;
            return count;
        }

        public  async Task<List<BasketProductDto>> GetBasketProducts(List<BasketItem> basketItems)
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

        public async Task<List<BasketProductDto>> GetDbBasketProducts(int userId)
        {
            
            return await GetBasketProducts(await _appDbContext.BasketItems.Where(x => x.UserId == userId).ToListAsync());


        }

        public async Task<bool> RemoveItemFromBasket(int productId, int productTypeId, int userId)
        {
            var item = await _appDbContext.BasketItems.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.ProductTypeId == productTypeId && x.UserId == userId);

            if (item == null)
            {
                return false;
            }
            _appDbContext.BasketItems.Remove(item);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<List<BasketProductDto>> StoreBasketItems(List<BasketItem> basketItems,int userId)
        {
            basketItems.ForEach( basketItem => basketItem.UserId = userId);
            _appDbContext.AddRange(basketItems);
            await _unitOfWork.CommitAsync();
            return await this.GetDbBasketProducts(userId);
        }

        public async Task<bool> UpdateQuantity(BasketItem basketItem, int userId)
        {
            basketItem.UserId = userId;
            var item = await _appDbContext.BasketItems.FirstOrDefaultAsync(x => x.ProductId == basketItem.ProductId
            && x.ProductTypeId == basketItem.ProductTypeId && x.UserId == basketItem.UserId);

            if (item == null)
            {
                return false;
            }
            item.Quantity = basketItem.Quantity;
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
