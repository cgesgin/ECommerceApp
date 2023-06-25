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
    public class ProductTypeService : GenericService<ProductType>, IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductTypeService(IGenericRepository<ProductType> repository, IUnitOfWork unitOfWork, IProductTypeRepository productTypeRepository) : base(repository, unitOfWork)
        {
            _productTypeRepository = productTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductType>> AddProductType(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            await this._productTypeRepository.AddAsync(productType);
            await _unitOfWork.CommitAsync();
            return await GetProductTypes();
        }

        public  async Task<List<ProductType>> GetProductTypes()
        {
            var productTypes = await _productTypeRepository.GetAll().ToListAsync() ;
            return productTypes;
        }

        public async Task<List<ProductType>> UpdateProductType(ProductType productType)
        {
            var dbProductType = await this._productTypeRepository.Where(x=>x.Id==productType.Id).FirstAsync();
            if (dbProductType == null)
            {
                return null;
            }

            dbProductType.Name = productType.Name;
            await _unitOfWork.CommitAsync();

            return await GetProductTypes();
        }
    }
}
