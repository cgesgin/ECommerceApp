using ECommerceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Repositories
{
    public interface IBasketItemRepository : IGenericRepository<BasketItem>
    {
        void RemoveRange(List<BasketItem> list);
    }
}
