    using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            this._orderService = orderService;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _orderService.PlaceOrder(userId);
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrder()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _orderService.GetOrder(userId);
            return CreateActionResult(ResponseDto<List<OrderShowDto>>.Success(200, result));
        }

        [HttpGet("GetOrderDetails/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _orderService.GetOrderDetails(orderId,userId);
            return CreateActionResult(ResponseDto<OrderDetailsDto>.Success(200, result));
        }

    }
}
