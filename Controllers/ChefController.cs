using Microsoft.AspNetCore.Mvc;
using ordermanagement.Service;
using ordermanagement.Dtos.Chef;
using System.Threading.Tasks;

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {
        private readonly ChefService _chefService;

        public ChefController(ChefService chefService)
        {
            _chefService = chefService;
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
            return NoContent(); 
        }
    }
}
