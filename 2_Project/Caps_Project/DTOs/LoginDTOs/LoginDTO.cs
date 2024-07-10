namespace Caps_Project.DTOs.LoginDTOs
{
    // Para VerificarCredenciales SP
    public class LoginDTO
    {
        public string IdEmpleado { get; set; } = null!;
        public byte[]? Contrasena { get; set; }

    }
}
