using ECommerceApp.Core.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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

        public async Task<string> PlaceOrder()
        {
            if (await GetIsAuthenticated())
            {
                var result = await _httpClient.PostAsync("api/payment/checkout", null);
                var url = await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }

        private async Task<bool> GetIsAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
