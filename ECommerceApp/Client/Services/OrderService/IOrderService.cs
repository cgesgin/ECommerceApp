using ECommerceApp.Core.Dto;

namespace ECommerceApp.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder();
        Task<List<OrderShowDto>> GetOrders();
        Task<OrderDetailsDto> GetDetailsOrders(int orderId);
    }
}
