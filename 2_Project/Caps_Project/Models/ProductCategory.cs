using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class ProductCategory
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
