using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IAddressService : IGenericService<Address>
    {
        Task<Address> GetAddress(int userId);
        Task<Address> AddOrUpdateAddress(Address address, int userId);

    }
}
