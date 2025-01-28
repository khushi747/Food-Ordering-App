using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class Orderdetail
{
    public int OrderId { get; set; }

    public string? ItemsDetails { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Order Order { get; set; } = null!;
}
