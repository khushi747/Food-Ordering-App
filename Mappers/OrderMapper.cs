//using Newtonsoft.Json;
//using ordermanagement.Dtos.User;
//using ordermanagement.Models;

//namespace ordermanagement.Mappers
//{
//    public static class OrderMapper
//    {
//        public static Order MapToOrder(CreateOrderDto dto)
//        {
//            var order = new Order
//            {
//                UserId = dto.UserId,
//                OrderDate = DateTime.Now,
//                TotalPrice = dto.Items.Sum(i => i.Quantity * GetMenuItemPrice(i.ItemId)) // Placeholder for actual item price logic.
//            };

//            var orderDetails = new Orderdetail
//            {
//                OrderId = order.OrderId,
//                TotalQuantity = dto.Items.Sum(i => i.Quantity),
//                TotalPrice = order.TotalPrice,
//                ItemsDetails = JsonConvert.SerializeObject(dto.Items)
//            };

//            //order.Orderdetails = new List<Orderdetail> { orderDetails };
//            return order;
//        }
//        //public int OrderId { get; set; }

//        //public string? ItemsDetails { get; set; }

//        //public int TotalQuantity { get; set; }

//        //public decimal TotalPrice { get; set; }

//        //public virtual Order Order { get; set; } = null!;

//        public static Orderstatus MapToOrderStatus(CancelOrderDto dto)
//        {
//            return new Orderstatus
//            {
//                OrderId = dto.OrderId,
//                Status = "Cancelled",
//                //StatusDate = DateTime.Now
//            };
//        }

//        private static decimal GetMenuItemPrice(int itemId)
//        {


//            return 0;
//        }
//    }
//}
