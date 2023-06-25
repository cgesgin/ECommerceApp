using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductController(IProductService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _service.GetProductsWithVariant();
            return CreateActionResult(ResponseDto<IEnumerable<Product>>.Success(200, result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {  
            var result = await _service.GetProductWithVariantAndType(id);
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

        [HttpGet("search/{searchText}/{page}")]
        public async Task<IActionResult> SearchProducts(string searchText,int page=1)
        {
            var result = await _service.SearchProducts(searchText,page);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<String>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<ProductSearchResultDto>.Success(200, result));
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<IActionResult> GetProductsSearchSuggestions(string searchText)
        {
            var result = await _service.GetProductsSearchSuggestions(searchText);
            if (result == null)
            {
                return CreateActionResult(ResponseDto<List<string>>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<string>>.Success(200, result));
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            var result = await _service.GetFeaturedProducts();
            if (result == null)
            {
                return CreateActionResult(ResponseDto<List<string>>.Fail(404, "Data not found"));
            }
            return CreateActionResult(ResponseDto<List<Product>>.Success(200, result));
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminProducts()
        {
            var result = await _service.GetAdminProducts();
            return CreateActionResult(ResponseDto<List<Product>>.Success(200, result));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var result = await _service.CreateProduct(product);
            return CreateActionResult(ResponseDto<Product>.Success(200, result));
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var result = await _service.UpdateProduct(product);
            return CreateActionResult(ResponseDto<Product>.Success(200, result));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _service.DeleteProduct(id);
            return CreateActionResult(ResponseDto<bool>.Success(200, result));
        }

    }
}