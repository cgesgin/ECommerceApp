using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            foreach (var variant in product.ProductVariants)
            {
                variant.ProductType = null;
            }
            await this._productRepository.AddAsync(product);
            await this._unitOfWork.CommitAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var dbProduct = await _productRepository.Where(x => x.Id == productId).FirstAsync();
            if (dbProduct == null)
            {
               return false;
            }

            dbProduct.Deleted = true;

            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<List<Product>> GetAdminProducts()
        {
            return await this._productRepository.GetProductsForAdmin();
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

        public Task<Product> UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }
    }
}
