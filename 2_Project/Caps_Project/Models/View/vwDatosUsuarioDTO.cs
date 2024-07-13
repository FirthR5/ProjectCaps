using System.ComponentModel.DataAnnotations;

namespace Caps_Project.Models.View.DTOs.LoginDTOs
{
    public class vwDatosUsuarioDTO
    {
        //TODO: Revisar -- Ver lista de empleados activos Linea 85 (3)
        [Key]
        public string IdEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public string EmpTypeName { get; set; }
    }
}
