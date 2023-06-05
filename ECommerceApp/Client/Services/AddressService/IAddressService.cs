
using ECommerceApp.Core.Models;

namespace ECommerceApp.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> GetAddressAsync();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
