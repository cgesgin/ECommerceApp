using ECommerceApp.Core.Dto;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
