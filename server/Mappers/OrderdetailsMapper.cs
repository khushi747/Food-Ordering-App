using System;
using ordermanagement.Dtos.User;
using ordermanagement.Models;

namespace ordermanagement.Mappers;

public static class OrderdetailsMapper
{
   public static OrderdetailsDto MapToOrderdetailsDto(this Orderdetail orderdetail)
       {
           return new OrderdetailsDto{
                 OrderId = orderdetail.OrderId ,
                 ItemsDetails = orderdetail.ItemsDetails,
                 TotalQuantity = orderdetail.TotalQuantity,
                 TotalPrice = orderdetail.TotalPrice,
                 OrderdetailId = orderdetail.OrderdetailId

           };
       }

       public static OrderdetailsForUserDto MapToOrderdetailsForUserDto(this Orderdetail orderdetail)
       {
           return new OrderdetailsForUserDto{

                 ItemsDetails = orderdetail.ItemsDetails,
                 TotalQuantity = orderdetail.TotalQuantity,
                 TotalPrice = orderdetail.TotalPrice,


           };
       }
}
