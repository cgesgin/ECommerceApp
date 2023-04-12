using ECommerceApp.Core.Models;
using ECommerceApp.Core.Repositories;
using ECommerceApp.Repository.Data;
using ECommerceApp.Repository.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Repository.Repositories
{
    public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
    {
        public BasketItemRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public void RemoveRange(List<BasketItem> list)
        {
            _appDbContext.RemoveRange(list);
        }
    }
}
