using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class ProductItem
{
    public int IdItem { get; set; }

    public int ProductPriceId { get; set; }

    public int Quantity { get; set; }

    public int TicketOrderId { get; set; }

    public int ProductId { get; set; }

    public virtual Producto Product { get; set; } = null!;

    public virtual ProductPrice ProductPrice { get; set; } = null!;

    public virtual OrderReceipt TicketOrder { get; set; } = null!;
}
