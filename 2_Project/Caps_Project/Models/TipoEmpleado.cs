using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class TipoEmpleado
{
    public int IdEmployeeType { get; set; }

    public string EmpTypeName { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
