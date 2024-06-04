using tbb.orders.api.Database;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using tbb.orders.api.Models;

namespace tbb.orders.api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContext _context;

        public OrderRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            var query = "SELECT * FROM Orders WHERE UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Order>(query, new { UserId = userId });
            }
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            var query = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            using (var connection = _context.CreateConnection())
            {
                var order = await connection.QuerySingleOrDefaultAsync<Order>(query, new { OrderId = orderId });
                if (order != null)
                {
                    var orderItemsQuery = "SELECT * FROM OrderItems WHERE OrderId = @OrderId";
                    order.OrderItems = (await connection.QueryAsync<OrderItem>(orderItemsQuery, new { OrderId = orderId })).ToList();
                }
                return order;
            }
        }

        public async Task<IEnumerable<Order>> SearchOrders(string criteria)
        {
            var query = "SELECT * FROM Orders WHERE Status LIKE @Criteria OR TotalAmount LIKE @Criteria";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Order>(query, new { Criteria = $"%{criteria}%" });
            }
        }

        public async Task<int> CreateOrder(Order order)
        {
            var query = "INSERT INTO Orders (UserId, OrderDate, Status, TotalAmount) VALUES (@UserId, @OrderDate, @Status, @TotalAmount); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                var orderId = await connection.QuerySingleAsync<int>(query, order);
                foreach (var item in order.OrderItems)
                {
                    item.OrderId = orderId;
                    var orderItemQuery = "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)";
                    await connection.ExecuteAsync(orderItemQuery, item);
                }
                return orderId;
            }
        }

        public async Task<int> ProcessRefund(Refund refund)
        {
            var query = "INSERT INTO Refunds (OrderId, RefundDate, Amount, Status) VALUES (@OrderId, @RefundDate, @Amount, @Status); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, refund);
            }
        }
    }
}
