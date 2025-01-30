using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual ICollection<Orderstatus> Orderstatuses { get; set; } = new List<Orderstatus>();

    public virtual User User { get; set; } = null!;
}
