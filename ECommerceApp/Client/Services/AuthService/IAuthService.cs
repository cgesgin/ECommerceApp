using ECommerceApp.Core.Dto;

namespace ECommerceApp.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ResponseDto<string>> Register(UserRegisterDto userRegisterDto);
        Task<ResponseDto<string>> Login(UserLoginDto userLoginDto);
        Task<ResponseDto<bool>> ChangePassword(UserChangePasswordDto userChangePasswordDto);
    }
}
