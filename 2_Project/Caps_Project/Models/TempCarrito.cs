using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class TempCarrito
{
    public int IdItem { get; set; }

    public int ProductPriceId { get; set; }

    public int Quantity { get; set; }

    public Guid OrderUuid { get; set; }

    public int ProductId { get; set; }
}
