using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caps_Project.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbCapsContext _contexto;
        public LoginController(DbCapsContext context)
        {
            _contexto = context;
        }
        // GET: LoginController

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            //LoginDTO objeto = new LoginDTO()
            //{
            //    IdEmpleado = "ADM-000001",
            //    Contrasena = "12345"
            //};
            var serv_Login = new LoginService(_contexto);
            int UsuarioCorrecto = await serv_Login.VerificarCredenciales(loginDTO);
            if (UsuarioCorrecto == 1)
            {
                int UsuarioActivo = await serv_Login.VerificarUsuarioActivo(loginDTO.IdEmpleado);
                if (UsuarioCorrecto == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Usuario No activo
                }
            }
            else
            {
                // return usuario no correcto
            }
            return View();
        }
    }
}
