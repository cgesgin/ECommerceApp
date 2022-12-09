using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class ProductService : GenericService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _productRepository = productRepository;
        }

        public Task<List<Product>> GetFeaturedProducts()
        {
            return _productRepository.GetFeaturedProducts();
        }

        public async Task<List<Product>> GetProductByCategoryAsync(string categoryName)
        {
            return await _productRepository.GetProductByCategoryAsync(categoryName);
        }

        public Task<List<string>> GetProductsSearchSuggestions(string searchText)
        {
            return _productRepository.GetProductsSearchSuggestions(searchText);
        }

        public async Task<List<Product>> GetProductsWithVariant()
        {
            return await _productRepository.GetProductsWithVariant();
        }

        public async Task<Product> GetProductWithVariantAndType(int id)
        {
            return await _productRepository.GetProductWithVariantAndType(id);
        }

        public Task<List<Product>> SearchProducts(string searchText)
        {
            return _productRepository.SearchProducts(searchText);
        }

        public async Task<ProductSearchResultDto> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount =Math.Ceiling((await _productRepository.SearchProducts(searchText)).Count/pageResults);
            List<Product> products = await _productRepository.SearchProducts(searchText, page, pageResults);
            return new ProductSearchResultDto { CurrentPage=page,Pages=(int)pageCount,Products=products };
        }
    }
}
