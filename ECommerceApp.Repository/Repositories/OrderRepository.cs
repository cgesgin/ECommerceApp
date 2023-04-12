using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<Order>> GetOrder(int userId)
        {            
             var data= await _appDbContext.Order.Include(x => x.OrderItems).ThenInclude(x => x.Product).Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate).ToListAsync();
            return data;
        }

        public async Task<Order> GetOrderDetails(int orderId, int userId)
        {
            var data =await _appDbContext.Order.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.OrderItems).ThenInclude(x => x.ProductType)
                .Where(x => x.UserId == userId && x.Id == orderId).OrderByDescending(x => x.OrderDate).FirstOrDefaultAsync();
            return data;
        }
    }
}
