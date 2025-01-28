using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class Menuitem
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }
}
