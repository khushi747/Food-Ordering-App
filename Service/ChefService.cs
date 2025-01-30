using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Dtos.Chef;
using ordermanagement.Models;

namespace ordermanagement.Service
{
    public class ChefService
    {
        private readonly GininternsContext _context;

        public ChefService(GininternsContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Select(order => new OrderDto
                {
                    OrderId = order.OrderId,
                    UserName = order.User.Name,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    OrderStatuses = order.Orderstatuses
                        .Select(status => new OrderStatusDto
                        {
                            StatusId = status.StatusId,
                            Status = status.Status,
                            //StatusDate = status.StatusDate
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderStatusDto?> GetOrderStatusAsync(int orderId)
        {
            var status = await _context.Orderstatuses
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusId)
                .FirstOrDefaultAsync();

            if (status == null) return null;

            return new OrderStatusDto
            {
                StatusId = status.StatusId,
                Status = status.Status,
                //StatusDate = status.StatusDate
            };
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            var newStatus = new Orderstatus
            {
                OrderId = orderId,
                Status = dto.Status,
                // Uncomment if StatusDate is required
                // StatusDate = DateTime.UtcNow
            };

            _context.Orderstatuses.Add(newStatus);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
