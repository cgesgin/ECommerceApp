using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _service.GetAllAsync();
            return CreateActionResult(ResponseDto<IEnumerable<Product>>.Success(200, result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result==null)
            {
                return CreateActionResult(ResponseDto<Product>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<Product>.Success(200, result));
        }

        [HttpGet("category/{categorName}")]
        public async Task<IActionResult> GetProductByCategory(string categorName)
        {
            var result = await _service.GetProductByCategoryAsync(categorName);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<List<Product>>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<Product>>.Success(200, result));
        }
    }
}