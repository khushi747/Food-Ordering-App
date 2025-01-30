using Newtonsoft.Json;
using ordermanagement.Data;
using ordermanagement.Dtos.User;
using ordermanagement.Mappers;
using ordermanagement.Models;

namespace ordermanagement.Service
{
    public class UserService(GininternsContext context)
    {
        private readonly GininternsContext _context = context;

        public decimal GetMenuItemPrice(int itemId)
        {
            var menuItem = _context.Menuitems.FirstOrDefault(m => m.ItemId == itemId);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"{itemId} not found. Please check the menuId properly");
            }

            return menuItem.Price;
        }
        public async Task<Order> CreateOrder(CreateOrderDto createOrderDto)
        {
                decimal TotalPrice = createOrderDto.Items.Sum(i => i.Quantity * GetMenuItemPrice(i.ItemId));

                var order = new Order
                {
                    UserId = createOrderDto.UserId,
                    OrderDate = DateTime.Now,
                    TotalPrice = TotalPrice
                };

                // Save Order first to generate OrderId
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Ensure order ID is generated!

                // Add Order Details
                var orderDetails = createOrderDto.Items.Select(item => new Orderdetail
                {
                    OrderId = order.OrderId, // Now OrderId is available!
                    TotalQuantity = item.Quantity,
                    TotalPrice = GetMenuItemPrice(item.ItemId) * item.Quantity,
                    ItemsDetails = JsonConvert.SerializeObject(item)
                }).ToList();

                _context.Orderdetails.AddRange(orderDetails);

                // Add Order Status
                var orderStatus = new Orderstatus
                {
                    OrderId = order.OrderId, // OrderId is correctly assigned
                    Status = "Pending"
                };

                _context.Orderstatuses.Add(orderStatus);

                // Save Order Details and Status
                await _context.SaveChangesAsync();

                return order;
            }

        public async Task SaveOrder(Order order)
        {
            var orderStatus = new Orderstatus
            {
                OrderId = order.OrderId,
                Status = "Pending"
            };
            await _context.Orders.AddAsync(order);
            await _context.Orderstatuses.AddAsync(orderStatus);
            await _context.SaveChangesAsync();
        }


        //public bool CancelOrder(CancelOrderDto dto)
        //{
        //    var order = _context.Orders.Find(dto.OrderId);
        //    if (order == null)
        //    {
        //        return false;
        //    }

        //var orderStatus = new Orderstatus
        //{
        //    OrderId = dto.OrderId,
        //    Status = "Cancelled",
        //    //StatusDate = DateTime.Now
        //};

        //_context.Orderstatuses.Add(orderStatus);
        //    _context.SaveChanges();
        //    return true;
        //}
    }
}

