using Newtonsoft.Json;
using ordermanagement.Dtos.User;
using ordermanagement.Models;
using System.Linq;

namespace ordermanagement.Mappers
{
    public static class OrderMapper
    {
        public static OrderStatusDto MapToOrderStatusDto(this Orderstatus orderstatus)
        {
            return new OrderStatusDto
            {
                StatusId = orderstatus.StatusId,
                OrderId = orderstatus.OrderId,
                Status = orderstatus.Status
            };
        }

        public static OrderDto MapToOrderDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Orderdetails = [.. order.Orderdetails.Select(i => i.MapToOrderdetailsDto())],
                Orderstatuses = order.Orderstatuses.Select(i => i.MapToOrderStatusDto()).ToList()
            };
        }

         public static OrderstatusForUserDto MapToOrderStatusForUserDto(this Orderstatus orderstatus)
        {
            return new OrderstatusForUserDto
            {
                Status = orderstatus.Status
            };
        }

        public static OrderDto MapToOrderForUserDto(this Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                //issues to solve phir kabhi 
                Orderdetails = order.Orderdetails.Select(i => i.MapToOrderdetailsDto()).ToList(), // to use maptoorderdetailsforuserdto ahead //issues to solve
                Orderstatuses = order.Orderstatuses.Select(i => i.MapToOrderStatusDto()).ToList()
            };
        }
    }
}
