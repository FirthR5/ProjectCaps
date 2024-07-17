using System;
using System.Collections.Generic;

namespace Caps_Project.Models;

public partial class VwListaEmpleado
{
    public string IdEmpleado { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string? EmpTypeName { get; set; }

    public string Estado { get; set; } = null!;

    public string? Turno { get; set; }
}
