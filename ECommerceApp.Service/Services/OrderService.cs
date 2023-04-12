using ECommerceApp.Core.Dto;
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
    public class OrderService : GenericService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitOfWork, IBasketService basketService, IOrderRepository orderRepository,IBasketItemRepository basketItemRepository) : base(repository, unitOfWork)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _basketItemRepository = basketItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderShowDto>> GetOrder(int userId)
        {
            var orders= await _orderRepository.GetOrder(userId);

            var ordersDto= new List<OrderShowDto>();

            orders.ForEach(o => ordersDto.Add(new OrderShowDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} -" + $"{o.OrderItems.Count - 1} -> " : o.OrderItems.First().Product.Title,
                Productİmage = o.OrderItems.First().Product.ImageUrl

            }));
            return ordersDto;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId, int userId)
        {
            var data = await _orderRepository.GetOrderDetails(orderId, userId);
            if (data==null)
            {
                return null;
            }
            var orderDetailsDto = new OrderDetailsDto {
                OrderDate = data.OrderDate,
                TotalPrice = data.TotalPrice,
                Products = new List<OrderDetailsProductDto>()
            };

            data.OrderItems.ForEach(item =>
                orderDetailsDto.Products.Add(new OrderDetailsProductDto
                {
                    ProductId = item.ProductId,
                    ImageUrl = item.Product.ImageUrl,
                    ProductType = item.ProductType.Name,
                    Quantity = item.Quantity,
                    Title = item.Product.Title,
                    TotalPrice = item.TotalPrice,
                }
            ));
            return orderDetailsDto;
        }

        public async Task<bool> PlaceOrder(int userId)
        {
            var products = (await _basketService.GetDbBasketProducts(userId));
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };
            await _orderRepository.AddAsync(order);
            _basketItemRepository.RemoveRange(_basketItemRepository.Where(ci => ci.UserId == userId).ToList());
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
