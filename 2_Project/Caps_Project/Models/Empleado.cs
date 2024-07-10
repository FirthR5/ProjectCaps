using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class Empleado
{
    public string IdEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApPaterno { get; set; } = null!;

    public string ApMaterno { get; set; } = null!;

    public byte[]? Contrasena { get; set; }

    public int EmployeeType { get; set; }

    public virtual ICollection<EmpleadoActivo> EmpleadoActivos { get; set; } = new List<EmpleadoActivo>();

    public virtual TipoEmpleado EmployeeTypeNavigation { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<OrderReceipt> OrderReceipts { get; set; } = new List<OrderReceipt>();
}
