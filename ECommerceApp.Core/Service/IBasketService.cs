using ECommerceApp.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Service
{
    public interface IBasketService
    {
        Task<List<BasketProductDto>> GetBasketProducts(List<BasketItemDto> basketItems);
    }
}
