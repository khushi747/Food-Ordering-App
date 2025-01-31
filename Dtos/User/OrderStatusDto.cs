using System;

namespace ordermanagement.Dtos.User;

public class OrderStatusDto
{
      public int StatusId { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; } = "Pending";
}
