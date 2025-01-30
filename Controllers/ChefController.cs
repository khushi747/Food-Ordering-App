using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ordermanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {
        private readonly GininternsContext _context;

        public ChefController(GininternsContext context)
        {
            _context = context;
        }

        // GET: api/chef/orders
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Orderstatuses)
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    UserName = o.User.Name,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Statuses = o.Orderstatuses.Select(s => new
                    {
                        StatusId = s.StatusId,
                        Status = s.Status,
                        //StatusDate = s.StatusDate
                    }).ToList()
                })
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            return Ok(orders);
        }

        // GET: api/chef/orders/{orderId}/status
        [HttpGet("orders/{orderId}/status")]
        public async Task<IActionResult> GetOrderStatus(int orderId)
        {
            var orderStatus = await _context.Orderstatuses
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusId)
                .Select(s => new
                {
                    StatusId = s.StatusId,
                    Status = s.Status,
                    //StatusDate = s.StatusDate
                })
                .FirstOrDefaultAsync();

            if (orderStatus == null)
            {
                return NotFound($"No status found for order with ID {orderId}.");
            }

            return Ok(orderStatus);
        }

        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest("Invalid status update data.");
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            var currentOrderStatus = await _context.Orderstatuses
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusId)
                .FirstOrDefaultAsync();

            if (currentOrderStatus == null)
            {
                return NotFound("Order status not found.");
            }

            currentOrderStatus.Status = dto.Status;
            //currentOrderStatus.StatusDate = DateTime.UtcNow; // Optional: to record the date when the status is updated

            await _context.SaveChangesAsync(); 

            return NoContent(); 
        }


    }

    // DTO for updating the order status
    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = null!;
    }
}
