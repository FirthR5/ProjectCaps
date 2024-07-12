namespace Caps_Project.DTOs.EmpleadoDTOs
{
    public class InsertEmpleadoDTO
    {

        public string Nombre { get; set; } = null!;

        public string ApPaterno { get; set; } = null!;

        public string ApMaterno { get; set; } = null!;

        public string Contrasena { get; set; }

        public int EmployeeType { get; set; }
        public string Turno { get; set; } = null!;
    }
}
