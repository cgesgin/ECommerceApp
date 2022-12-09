using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly IBasketService _service;
        public BasketController(IBasketService service)
        {
            _service = service;
        }

        [HttpPost("products")]
        public async Task<IActionResult> GetBasketProducts(List<BasketItemDto> basketItems) 
        {
            var result = await _service.GetBasketProducts(basketItems);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<BasketProductDto>>.Success(200, result));
        }
    }
}
