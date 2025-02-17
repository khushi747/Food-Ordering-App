using System;

namespace ordermanagement.Dtos.User;

public class OrderdetailsDto
{
    public int OrderId { get; set; }

    public string? ItemsDetails { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalPrice { get; set; }

    public int OrderdetailId { get; set; }

    // public virtual Order Order { get; set; } = null!;
}

public class OrderdetailsForUserDto{

    public string? ItemsDetails { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalPrice { get; set; }


}
