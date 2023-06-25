using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {

        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetCategories();
            if (result == null)
            {
                return CreateActionResult(ResponseDto<IEnumerable<Category>>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<IEnumerable<Category>>.Success(200, result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<Category>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<Category>.Success(200, result));
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminCategories()
        {
            var result = await _service.GetAdminCategories();
            return CreateActionResult(ResponseDto<List<Category>>.Success(200, result));
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _service.DeleteCategory(id);
            return CreateActionResult(ResponseDto<List<Category>>.Success(200, result));
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            var result = await _service.AddCategory(category);
            return CreateActionResult(ResponseDto<List<Category>>.Success(200, result));
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var result = await _service.UpdateCategory(category);
            return CreateActionResult(ResponseDto<List<Category>>.Success(200, result));
        }
    }
}
