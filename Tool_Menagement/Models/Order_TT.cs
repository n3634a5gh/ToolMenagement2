using System;
using System.Collections.Generic;
using Tool_Menagement.Models;

namespace Tool_Menagement.Models;
public partial class OrderTT
{
    public int PositionId { get; set; }

    public int ToolId { get; set; }

    public bool Active { get; set; }

    public int OrderId { get; set; }

    public virtual Zlecenie Order { get; set; } = null!;
}
