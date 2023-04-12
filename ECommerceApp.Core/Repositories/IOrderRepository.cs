using ECommerceApp.Core.Dto;
using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetOrder(int userId);
        Task<Order> GetOrderDetails(int orderId, int userId);
    }
}
