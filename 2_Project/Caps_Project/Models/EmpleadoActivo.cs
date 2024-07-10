using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class EmpleadoActivo
{
    public int Id { get; set; }

    public string IdEmpleado { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Turno { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
