using ECommerceApp.Core.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient httpClient,AuthenticationStateProvider authenticationStateProvider,NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsDto> GetDetailsOrders(int orderId)
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<OrderDetailsDto>>($"api/Order/GetOrderDetails/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderShowDto>> GetOrders()
        {
            var result = await _httpClient.GetFromJsonAsync<ResponseDto<List<OrderShowDto>>>("api/Order/GetOrders");
            return result.Data;
        }

        public async Task PlaceOrder()
        {
            if (await this.GetIsAuthenticated())
            {
                await _httpClient.PostAsync("api/Order/PlaceOrder", null);
            }
            else
            {
                _navigationManager.NavigateTo("login");
            }
        }

        private async Task<bool> GetIsAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
