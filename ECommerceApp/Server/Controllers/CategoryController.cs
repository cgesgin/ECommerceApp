using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
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
            var result = await _service.GetAllAsync();
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

    }
}
