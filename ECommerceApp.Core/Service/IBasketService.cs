using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IBasketService
    {
        Task<List<BasketProductDto>> GetBasketProducts(List<BasketItem> basketItems);
        Task<List<BasketProductDto>> StoreBasketItems(List<BasketItem> basketItems,int userId);
        Task<int> GetBasketItems(int userId);
        Task<List<BasketProductDto>> GetDbBasketProducts(int userId);
        Task<bool> AddToBasket(BasketItem basketItem,int userId);
        Task<bool> UpdateQuantity(BasketItem basketItem,int userId);
        Task<bool> RemoveItemFromBasket(int productId,int productTypeId, int userId);
    }
}
