using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class OrderReceipt
{
    public int OrderId { get; set; }

    public string IdEmpleado { get; set; } = null!;

    public decimal TotalPaid { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
