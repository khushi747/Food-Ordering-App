using System;
using System.Collections.Generic;

namespace ordermanagement.Models;

public partial class Locker
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string TypeOfLocker { get; set; } = null!;

    public bool LockeStatus { get; set; }

    public int? UserId { get; set; }
}
