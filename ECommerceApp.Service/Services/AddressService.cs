using ECommerceApp.Core.Repositories;
using ECommerceApp.Core.Service;
using ECommerceApp.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Service.Services
{
    public class AddressService : GenericService<Core.Models.Address>,IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IGenericRepository<Core.Models.Address> repository, IUnitOfWork unitOfWork,IAddressRepository addressRepository) : base(repository, unitOfWork)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Core.Models.Address> AddOrUpdateAddress(Core.Models.Address address,int userId)
        {
            Core.Models.Address responseData =null;
            var addressdb = await this.GetAddress(userId);
            if (addressdb == null)
            {   
                address.UserId = userId;
                await _addressRepository.AddAsync(address);
                responseData = address;
            }
            else
            {
                addressdb.FirstName = address.FirstName;
                addressdb.LastName = address.LastName;
                addressdb.State = address.State;
                addressdb.City = address.City;
                addressdb.Country = address.Country;
                addressdb.Zip = address.Zip;
                addressdb.Street = address.Street;
                responseData=addressdb;

            }
             await _unitOfWork.CommitAsync();

            return responseData;
        }

        public async Task<Core.Models.Address> GetAddress(int userId)
        {
            var address = await _addressRepository.Where(x=>x.UserId==userId).FirstAsync();
            return address;
        }
    }
}
