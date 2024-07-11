namespace Caps_Project.DTOs.EmpleadoDTOs
{
    public class InsertEmpleadoDTO
    {

        public string Nombre { get; set; } = null!;

        public string ApPaterno { get; set; } = null!;

        public string ApMaterno { get; set; } = null!;

        public byte[]? Contrasena { get; set; }

        public int EmployeeType { get; set; }
    }
}
