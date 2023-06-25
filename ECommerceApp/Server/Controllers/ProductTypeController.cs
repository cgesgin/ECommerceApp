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
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : BaseController
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductTypes()
        {
            var result = await _productTypeService.GetProductTypes();
            return CreateActionResult(ResponseDto<List<ProductType>>.Success(200, result));
        }

        [HttpPost]
        public async Task<IActionResult> AddProductType(ProductType productType)
        {
            var result = await _productTypeService.AddProductType(productType);
            return CreateActionResult(ResponseDto<List<ProductType>>.Success(200, result));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductType(ProductType productType)
        {
            var result = await _productTypeService.UpdateProductType(productType);
            return CreateActionResult(ResponseDto<List<ProductType>>.Success(200, result));
        }
    }
}
