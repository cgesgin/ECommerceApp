using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using ECommerceApp.Repository.Migrations;
using ECommerceApp.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController(IPaymentService service,IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("checkout"), Authorize]
        public async Task<ActionResult<string>> CreateCheckoutSession()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var result = await _service.CreateCheckoutSession(userId, email);
            return Ok(result.Url);
        }


        [HttpPost]
        public async Task<IActionResult> FullFillOrder()
        {
            var result = await _service.FulfillOrder(Request);
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }
    }
}
