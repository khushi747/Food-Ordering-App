using Newtonsoft.Json;
using ordermanagement.Data;
using ordermanagement.Dtos.User;
using ordermanagement.Mappers;
using ordermanagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using ZstdSharp.Unsafe;

namespace ordermanagement.Service
{
    public class UserService
    {
        private readonly GininternsContext _context;

        public UserService(GininternsContext context)
        {
            _context = context;
        }

        public decimal GetMenuItemPrice(int itemId)
        {
            var menuItem = _context.Menuitems.FirstOrDefault(m => m.ItemId == itemId);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Item ID {itemId} not found. Please check the menu properly.");
            }

            return menuItem.Price;
        }

        public async Task<OrderDto>  CreateOrder(CreateOrderDto createOrderDto)
        {
            decimal totalPrice = 0;
            List<Menuitem> itemsToUpdate = new();

            foreach (var item in createOrderDto.Items)
            {
                var menuItem = await _context.Menuitems.FirstOrDefaultAsync(m => m.ItemId == item.ItemId);

        if (menuItem == null)
        {
        
            // return BadRequest( { success = false, message = $"Item ID {item.ItemId} not found." });
        }

        if (menuItem.Quantity < item.Quantity)
        {
            // return BadRequest(new { success = false, message = $"Item ID {item.ItemId} does not have enough stock." });
        }

                totalPrice += item.Quantity * menuItem.Price;
                menuItem.Quantity -= item.Quantity; 
                itemsToUpdate.Add(menuItem);
            }

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderDetails = createOrderDto.Items.Select(item => new Orderdetail
            {
                OrderId = order.OrderId,
                TotalQuantity = item.Quantity,
                TotalPrice = GetMenuItemPrice(item.ItemId) * item.Quantity,
                ItemsDetails = JsonConvert.SerializeObject(item)
            }).ToList();

            _context.Orderdetails.AddRange(orderDetails);

            var orderStatus = new Orderstatus
            {
                OrderId = order.OrderId,
                Status = "Pending"
            };

            _context.Orderstatuses.Add(orderStatus);
            _context.Menuitems.UpdateRange(itemsToUpdate);

            await _context.SaveChangesAsync();

            return order.MapToOrderDto() ;
        }


        public async Task<object> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Orderdetails).FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return new { success = false, message = "Order does not exist" };
            }



            foreach (var orderDetail in order.Orderdetails)
            {
                var itemDetails = JsonConvert.DeserializeObject<OrderItemDto>(orderDetail.ItemsDetails);

                if (itemDetails != null)
                {
                    var menuItem = await _context.Menuitems.FirstOrDefaultAsync(m => m.ItemId == itemDetails.ItemId);

                    if (menuItem != null)
                    {

                        menuItem.Quantity += itemDetails.Quantity; 
                        _context.Menuitems.Update(menuItem);
                    }
                }
            }
            _context.Orders.Remove(order);

            var orderStatus = new Orderstatus
            {
                OrderId = orderId,
                Status = "Cancelled"
            };

            _context.Orderstatuses.Add(orderStatus);
            await _context.SaveChangesAsync();

            return new { success = true, message = "your order is cancelled" };
        }

       public async Task<IEnumerable<OrderDto>> GetOrdersByUid(int id)
       {    

            var orders = await _context.Orders
                .Include(o => o.Orderstatuses)
                .Include(o => o.Orderdetails)
                .Where(o => o.UserId == id )
                .ToListAsync();

         return orders.Select(o => o.MapToOrderForUserDto()).ToList();     

       }    

    }
}
