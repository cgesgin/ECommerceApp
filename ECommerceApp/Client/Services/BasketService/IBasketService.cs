using ECommerceApp.Core.Dto;

namespace ECommerceApp.Client.Services.BasketService
{
    public interface IBasketService
    {
        event Action OnChange;
        Task AddToBasket(BasketItemDto basketItemDto);
        Task<List<BasketItemDto>> GetBasketItemsAsync();
        Task<List<BasketProductDto>> GetBasketProducts();
        Task RemoveProductFromBasket(int productId,int productTypeId);
        Task UpdateQuantity(BasketProductDto basketProductDto);
    }
}
