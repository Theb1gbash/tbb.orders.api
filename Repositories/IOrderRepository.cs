using System.Collections.Generic;
using System.Threading.Tasks;
using tbb.orders.api.Models;

namespace tbb.orders.api.Repositories
{
    public interface IOrderRepository
    {
        // Existing order-related methods
        Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
        Task<Order?> GetOrderById(int orderId);
        Task<IEnumerable<Order>> SearchOrders(string criteria);
        Task<int> CreateOrder(Order order);
        Task<int> ProcessRefund(Refund refund);

        // New ticket-related methods
        Task<IEnumerable<Ticket>> GetTicketsByOrderIdAsync(int orderId);
        Task<Ticket?> GetTicketByIdAsync(int ticketId);
        Task<int> CreateTicketAsync(Ticket ticket);
        Task<bool> UpdateTicketAsync(Ticket ticket);
        Task<bool> DeleteTicketAsync(int ticketId);

    }
}
