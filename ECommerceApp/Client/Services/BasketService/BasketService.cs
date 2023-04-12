using Blazored.LocalStorage;
using ECommerceApp.Client.Pages;
using ECommerceApp.Client.Services.AuthService;
using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.BasketService
{

    public class BasketService : IBasketService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient httpClient;
        private readonly IAuthService _authService;

        public event Action OnChange;

        public BasketService(ILocalStorageService localStorage,HttpClient httpClient, IAuthService authService)
        {
            _localStorage = localStorage;
            this.httpClient = httpClient;
            this._authService = authService;
        }

        public BasketService()
        {
        }

        public async Task AddToBasket(BasketItem basketItemDto)
        {

            if (await _authService.GetIsUserAuthentication())
            {
                await httpClient.PostAsJsonAsync("api/Basket/add",basketItemDto);
            }
            else
            {
                var basket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
                if (basket == null)
                {
                    basket = new List<BasketItem>();
                }
             
                var sameItem = basket.Find(x => x.ProductId == basketItemDto.ProductId && x.ProductTypeId == basketItemDto.ProductTypeId);
                
                if (sameItem == null)
                {
                    basket.Add(basketItemDto);
                }
                else
                {
                    sameItem.Quantity += basketItemDto.Quantity;
                }

                await _localStorage.SetItemAsync("basket", basket);
            }
            await this.BasketItemsCount();
        }


        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            await this.BasketItemsCount();
            var basket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
            if (basket == null)
            {
                basket = new List<BasketItem>();
            }
            return basket;
        }

        public async Task<List<BasketProductDto>> GetBasketProducts()
        {
            if (await this._authService.GetIsUserAuthentication())
            {
                var response = await httpClient.GetFromJsonAsync<ResponseDto<List<BasketProductDto>>>("api/Basket");
                return response.Data;
            }
            else
            {
                var basketItems = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
                if (basketItems == null)
                {
                    return new List<BasketProductDto>();
                }
                var response = await httpClient.PostAsJsonAsync("api/Basket/products", basketItems);
                var basketProducts = await response.Content.ReadFromJsonAsync<ResponseDto<List<BasketProductDto>>>();

                return basketProducts.Data;
            }
        }

        public async Task RemoveProductFromBasket(int productId, int productTypeId)
        {
            if (await this._authService.GetIsUserAuthentication())
            {
                await httpClient.DeleteAsync($"api/Basket/{productId}/{productTypeId}");
            }
            else
            {
                var basket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
                if (basket == null)
                {
                    return;
                }
                var basketItem = basket.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);
                if (basketItem != null)
                {
                    basket.Remove(basketItem);
                    await _localStorage.SetItemAsync("basket", basket);

                }
            }
        }
        public async Task UpdateQuantity(BasketProductDto basketProductDto)
        {
            if (await this._authService.GetIsUserAuthentication())
            {
                var request = new BasketItem
                {
                    ProductId = basketProductDto.ProductId,
                    Quantity = basketProductDto.Quantity,
                    ProductTypeId = basketProductDto.ProductTypeId,
                };
                await httpClient.PutAsJsonAsync("/api/Basket/update-quantity",request);
            }
            else
            {
                var basket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
                if (basket == null)
                {
                    return;
                }
                var basketItem = basket.Find(x => x.ProductId == basketProductDto.ProductId
                && x.ProductTypeId == basketProductDto.ProductTypeId);
                if (basketItem != null)
                {
                    basketItem.Quantity = basketProductDto.Quantity;
                    await _localStorage.SetItemAsync("basket", basket);
                }
            }
        }

        public async Task StoreBasketItems(bool emptyLocalBasket=false)
        {
            var localBasket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
            if (localBasket == null)
            {
                return;
            }

            await httpClient.PostAsJsonAsync("api/Basket", localBasket);

            if (emptyLocalBasket)
            {
                await _localStorage.RemoveItemAsync("basket");
            }
        }

        public async Task BasketItemsCount()
        {
            if (await _authService.GetIsUserAuthentication())
            {
                var result = await httpClient.GetFromJsonAsync<ResponseDto<int>>("api/Basket/Count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("basketItemsCount",count);
            }
            else
            {
                var localbasket = await _localStorage.GetItemAsync<List<BasketItem>>("basket");
                await _localStorage.SetItemAsync<int>("basketItemsCount", localbasket !=null ?localbasket.Count : 0);

            }
            OnChange.Invoke();
        }
    }
}
