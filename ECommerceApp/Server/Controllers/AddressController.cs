using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddressController(IAddressService addressService,IHttpContextAccessor httpContextAccessor)
        {
            this._addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddress()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _addressService.GetAddress(userId);
            return CreateActionResult(ResponseDto<Core.Models.Address>.Success(200, result));
        }


        [HttpPost]
        public async Task<IActionResult> AddOrUpdateAddress(Core.Models.Address address)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _addressService.AddOrUpdateAddress(address,userId);
            return CreateActionResult(ResponseDto<Core.Models.Address>.Success(200, result));
        }


    }
}
