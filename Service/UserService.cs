using Newtonsoft.Json;
using ordermanagement.Data;
using ordermanagement.Dtos.User;
using ordermanagement.Mappers;
using ordermanagement.Models;

namespace ordermanagement.Service
{
    public class UserService
    {
        private readonly OrdermanagementdbContext _context;

        public UserService(OrdermanagementdbContext context)
        {
            _context = context;
        }

  
        public decimal GetMenuItemPrice(int itemId)
        {
            var menuItem = _context.Menuitems.FirstOrDefault(m => m.ItemId == itemId);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {itemId} not found.");
            }

            return menuItem.Price;
        }
        public Order CreateOrder(CreateOrderDto createOrderDto)
        {
            decimal TotalPrice = createOrderDto.Items.Sum(i => i.Quantity * GetMenuItemPrice(i.ItemId));

            var orderDetails = createOrderDto.Items.Select(item => new Orderdetail
            {
                TotalQuantity = item.Quantity,
                TotalPrice = GetMenuItemPrice(item.ItemId) * item.Quantity,
                ItemsDetails = JsonConvert.SerializeObject(item) 
            }).ToList();

   
            var order =  new Order
            {
                UserId = createOrderDto.UserId,
                OrderDate = DateTime.Now,
                TotalPrice = TotalPrice,
                Orderdetails = orderDetails
            };

            // Add the order to the context
           //await _context.Orders.AddAsync(order);
           // await _context.SaveChangesAsync(); // Save the order and its details

            var orderStatus = new Orderstatus
            {
                OrderId = order.OrderId,
                Status = "Pending"
            };

            //_context.Orderstatuses.Add(orderStatus);
            //_context.SaveChanges(); // Save the order status

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

