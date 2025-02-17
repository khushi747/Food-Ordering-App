using Microsoft.EntityFrameworkCore;
using ordermanagement.Data;
using ordermanagement.Dtos.Chef;
using ordermanagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ordermanagement.Service
{
    public class ChefService
    {
        private readonly GininternsContext _context;

        public ChefService(GininternsContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDto>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.Orderstatuses)
                .Include(o => o.User)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserName = o.User.Name, 
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatuses = o.Orderstatuses.Select(s => new OrderStatusDto
                    {
                        StatusId = s.StatusId,
                        Status = s.Status
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<ViewOrderitemsDto>> GetOrderItems(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Orderdetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return null;

            return order.Orderdetails.Select(d => new ViewOrderitemsDto
            {
                OrderId = d.OrderId,
                ItemsDetails = d.ItemsDetails,
                TotalQuantity = d.TotalQuantity
            }).ToList();
        }

        public async Task<OrderStatusDto> GetOrderStatus(int orderId)
        {
            var status = await _context.Orderstatuses
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusId)
                .FirstOrDefaultAsync();

            if (status == null) return null;

            return new OrderStatusDto
            {
                StatusId = status.StatusId,
                Status = status.Status
            };
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string newStatus)
        {
            var existingOrderStatus = await _context.Orderstatuses.FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (existingOrderStatus == null)
            {
                return false;
            }

        existingOrderStatus.Status = newStatus;
        var result = await _context.SaveChangesAsync();
        return result > 0;
        }
    }
}
