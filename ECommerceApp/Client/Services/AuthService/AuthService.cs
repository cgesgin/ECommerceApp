using ECommerceApp.Core.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationState;

        public AuthService(HttpClient httpClient,AuthenticationStateProvider authenticationState)
        {
            _httpClient = httpClient;
            this._authenticationState = authenticationState;
        }
        public async Task<ResponseDto<string>> Register(UserRegisterDto userRegisterDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Auth/register", userRegisterDto);
            return await response.Content.ReadFromJsonAsync<ResponseDto<string>>();
        }
        
        public async Task<ResponseDto<string>> Login(UserLoginDto userLoginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Auth/login", userLoginDto);
            return await response.Content.ReadFromJsonAsync<ResponseDto<string>>();
        }

        public async Task<ResponseDto<bool>> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Auth/change-password", userChangePasswordDto.Password);
            return await response.Content.ReadFromJsonAsync<ResponseDto<bool>>();
        }

        public async Task<bool> GetIsUserAuthentication()
        {
            return (await _authenticationState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}