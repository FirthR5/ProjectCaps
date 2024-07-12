using Caps_Project.Models.View.DTOs.LoginDTOs;

namespace Caps_Project.DTOs.EmpleadoDTOs
{
    public class EmpleadosMetaDTOs: vwDatosUsuarioDTO
    {
        public DateTime? EndDate { get; set; }
        public string Turno { get; set; } = null!;
    }
}
