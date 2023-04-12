using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var user = new User { Email = userRegisterDto.Email};
            var result = await _service.Register(user,userRegisterDto.Password);
            if (result == 0)
            {
                return CreateActionResult(ResponseDto<string>.Success(404, "Fail"));
            }
            return CreateActionResult(ResponseDto<string>.Success(200, "Success"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {  
            var result = await _service.Login(userLoginDto.Email, userLoginDto.Password);
            if (result == null || result.Equals("Fail Password"))
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "null data or password error"));
            }
            var data = ResponseDto<string>.Success(200, result);
            return CreateActionResult(data);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.ChangePassword(int.Parse(userId), newPassword);
            if (result == null || result==false)
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "failed"));
            }
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }
    }
}
