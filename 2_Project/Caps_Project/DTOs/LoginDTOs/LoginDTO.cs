using System.ComponentModel.DataAnnotations;

namespace Caps_Project.DTOs.LoginDTOs
{
    // Para VerificarCredenciales SP
    public class LoginDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(10, MinimumLength =10, ErrorMessage = "El nombre de usuario debe ser 10 caracteres.")]
        public string UserName { get; set; } 
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "La contraseña debe tener al menos 5 caracteres.")]
        public string Contrasena { get; set; }

    }
}
