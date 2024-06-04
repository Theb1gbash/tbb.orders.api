using System.Collections.Generic;
using System.Threading.Tasks;
using tbb.orders.api.Models;

namespace tbb.orders.api.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
        Task<Order?> GetOrderById(int orderId);
        Task<IEnumerable<Order>> SearchOrders(string criteria);
        Task<int> CreateOrder(Order order);
        Task<int> ProcessRefund(Refund refund);
    }
}
