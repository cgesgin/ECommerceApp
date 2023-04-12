using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly IBasketService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasketController(IBasketService service,IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("products")]
        public async Task<IActionResult> GetBasketProducts(List<BasketItem> basketItems) 
        {
            var result = await _service.GetBasketProducts(basketItems);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<BasketProductDto>>.Success(200, result));
        }

        [HttpPost("add")]
        public async Task<IActionResult> addToBasket(BasketItem basketItems)
        {

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.AddToBasket(basketItems,userId);
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }

        [HttpPost]
        public async Task<IActionResult> StoreBasketItems(List<BasketItem> basketItems)
        {
            var userId= int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.StoreBasketItems(basketItems,userId);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<BasketProductDto>>.Success(200, result));
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity(BasketItem basketItems)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.UpdateQuantity(basketItems, userId);
            if (result == false)
            {
                return CreateActionResult(ResponseDto <bool>.Success(404, result));
            }
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<IActionResult> RemoveItemFromBasket(int productId,int productTypeId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.RemoveItemFromBasket(productId,productTypeId, userId);
            if (result == false)
            {
                return CreateActionResult(ResponseDto<bool>.Success(404, result));
            }
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }

        [HttpGet("Count")]
        public async Task<IActionResult> GetBasketItemsCount()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.GetBasketItems(userId);
            return CreateActionResult(ResponseDto<int>.Success(200, result));
        }


        [HttpGet]
        public async Task<IActionResult> GetDbBasketProducts()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _service.GetDbBasketProducts(userId);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<string>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto <List<BasketProductDto>>.Success(200, result));
        }
    }
}
