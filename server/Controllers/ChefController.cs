using Microsoft.AspNetCore.Mvc;
using ordermanagement.Service;
using ordermanagement.Dtos.Chef;
using System.Threading.Tasks;
using ordermanagement.Data;
using Microsoft.EntityFrameworkCore;
using MassTransit.Transports;
using ordermanagement.Events;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ordermanagement.Producer;


namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {
        private readonly ChefService _chefService;
        private readonly IOrderStatusPublisher _orderStatusPublisher;
        private readonly GininternsContext _context;


        public ChefController(ChefService chefService,IOrderStatusPublisher orderStatusPublisher,GininternsContext context)
        {
            _chefService = chefService;
            _orderStatusPublisher = orderStatusPublisher;
            _context = context;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _chefService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("orders/{orderId}/items")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var orderItems = await _chefService.GetOrderItems(orderId);
            if (orderItems == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            return Ok(orderItems);
        }

        [HttpGet("orders/status/{orderId}")]
        public async Task<IActionResult> GetOrderStatus(int orderId)
        {
            var status = await _chefService.GetOrderStatus(orderId);
            if (status == null)
            {
                return NotFound(new { message = "Order status not found" });
            }
            return Ok(status);
        }

        [HttpPut("orders/status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            var updated = await _chefService.UpdateOrderStatus(orderId, dto.Status);
            if (!updated)
            {
                return NotFound(new { message = "Order not found or status not updated" });
            }
    
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            
            if(order ==  null){
                return Ok(new{message = "no order found"});
            }
            var userId = order.UserId;

            var orderEvent = new OrderStatusUpdatedEvent
            {
                OrderId = orderId,
                Status = dto.Status,
                UserId = userId
            };

            await _orderStatusPublisher.PublishOrderStatusUpdate(orderEvent);

            Console.WriteLine("Order status updated and event published");
            return Ok(new { message = "Order status updated and event published" });
        }
    }
}
