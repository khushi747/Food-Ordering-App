using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ordermanagement.Dtos.Chef;

namespace ordermanagement.Dtos.User;

public class OrderDto
{
    public int OrderId { get; set; }

    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual ICollection<OrderdetailsDto> Orderdetails { get; set; } = [];
    public virtual ICollection<OrderStatusDto> Orderstatuses { get; set; } = [];
}
