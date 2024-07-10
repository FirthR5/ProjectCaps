using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string ProdName { get; set; } = null!;

    public int Stock { get; set; }

    public int IdProdCategory { get; set; }

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ProductCategory IdProdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}
