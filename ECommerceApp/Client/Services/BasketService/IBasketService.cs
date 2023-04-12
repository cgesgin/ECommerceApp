using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;

namespace ECommerceApp.Client.Services.BasketService
{
    public interface IBasketService
    {
        event Action OnChange;
        Task AddToBasket(BasketItem basketItemDto);
        Task<List<BasketItem>> GetBasketItemsAsync();
        Task<List<BasketProductDto>> GetBasketProducts();
        Task RemoveProductFromBasket(int productId,int productTypeId);
        Task UpdateQuantity(BasketProductDto basketProductDto);
        Task StoreBasketItems(bool emptyLocalBasket);
        Task BasketItemsCount();
    }
}
