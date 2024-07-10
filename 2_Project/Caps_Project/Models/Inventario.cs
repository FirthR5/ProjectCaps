using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class Inventario
{
    public int Id { get; set; }

    public DateTime EntryDate { get; set; }

    public int Quantity { get; set; }

    public string IdAdmin { get; set; } = null!;

    public int IdProduct { get; set; }

    public virtual Empleado IdAdminNavigation { get; set; } = null!;

    public virtual Producto IdProductNavigation { get; set; } = null!;
}
