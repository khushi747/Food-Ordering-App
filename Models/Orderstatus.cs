using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class Orderstatus
{
    public int StatusId { get; set; }

    public int OrderId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
