using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using tbb.orders.api.Models;
using tbb.orders.api.Repositories;

namespace tbb.orders.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchOrders(string criteria)
        {
            var orders = await _orderRepository.SearchOrders(criteria);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            var orderId = await _orderRepository.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, order);
        }

        [HttpPost("refund")]
        public async Task<IActionResult> ProcessRefund([FromBody] Refund refund)
        {
            if (refund == null)
            {
                return BadRequest();
            }

            var refundId = await _orderRepository.ProcessRefund(refund);
            return Ok(refundId);
        }
    }
}
