using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class ProductPrice
{
    public int IdPrice { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int ProductId { get; set; }

    public virtual Producto Product { get; set; } = null!;

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
