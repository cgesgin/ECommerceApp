using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Address",address);
            return response.Content.ReadFromJsonAsync<ResponseDto<Address>> ().Result.Data;
        }

        public async Task<Address> GetAddressAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<Address>>("/api/Address");
            return response.Data;
        }
    }
}
