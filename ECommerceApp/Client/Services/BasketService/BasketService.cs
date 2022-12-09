using Blazored.LocalStorage;
using ECommerceApp.Core.Dto;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.BasketService
{

    public class BasketService : IBasketService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient httpClient;

        public event Action OnChange;

        public BasketService(ILocalStorageService localStorage,HttpClient httpClient)
        {
            _localStorage = localStorage;
            this.httpClient = httpClient;
        }

        public BasketService()
        {
        }

        public async Task AddToBasket(BasketItemDto basketItemDto)
        {
            var basket = await _localStorage.GetItemAsync <List<BasketItemDto>>("basket");
            if (basket==null)
            {
                basket=new List<BasketItemDto>();
            }
            var sameItem = basket.Find(x=>x.ProductId==basketItemDto.ProductId && x.ProductTypeId==basketItemDto.ProductTypeId);
            if (sameItem==null)
            {
                basket.Add(basketItemDto);
            }
            else
            {
                sameItem.Quantity += basketItemDto.Quantity;
            }
            

            await _localStorage.SetItemAsync("basket",basket);

            OnChange.Invoke();
        }

        public async Task<List<BasketItemDto>> GetBasketItemsAsync()
        {
            var basket = await _localStorage.GetItemAsync<List<BasketItemDto>>("basket");
            if (basket == null)
            {
                basket = new List<BasketItemDto>();
            }
            return basket;
        }

        public async Task<List<BasketProductDto>> GetBasketProducts()
        {
            var basketItems = await _localStorage.GetItemAsync<List<BasketItemDto>>("basket");
            var response = await httpClient.PostAsJsonAsync("api/Basket/products",basketItems);
            var basketProducts= await response.Content.ReadFromJsonAsync<ResponseDto<List<BasketProductDto>>>();
            
            return basketProducts.Data;
        }

        public async Task RemoveProductFromBasket(int productId, int productTypeId)
        {
            var basket =await _localStorage.GetItemAsync<List<BasketItemDto>>("basket");
            if (basket==null)
            {
                return;
            }
            var basketItem = basket.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
            if(basketItem != null)
            {
                basket.Remove(basketItem);
                await _localStorage.SetItemAsync("basket",basket);
                OnChange.Invoke();
                    
            }
                
        }
        public async Task UpdateQuantity(BasketProductDto basketProductDto)
        {
            var basket = await _localStorage.GetItemAsync<List<BasketItemDto>>("basket");
            if (basket == null)
            {
                return;
            }
            var basketItem = basket.Find(x => x.ProductId == basketProductDto.ProductId 
            && x.ProductTypeId == basketProductDto.ProductTypeId);
            if (basketItem != null)
            {
                basketItem.Quantity= basketProductDto.Quantity;
                await _localStorage.SetItemAsync("basket", basket);
            }
        }
    }
}
