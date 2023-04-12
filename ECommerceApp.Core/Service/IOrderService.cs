using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IOrderService : IGenericService<Order>
    {
        Task<bool> PlaceOrder(int userId);
        Task<List<OrderShowDto>> GetOrder(int userId);
        Task<OrderDetailsDto> GetOrderDetails(int orderId,int userId);

    }
}
