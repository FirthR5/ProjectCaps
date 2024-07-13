using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Caps_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        //private readonly IConfiguration _configuration;
        private readonly DbCapsContext contexto;
        public HomeController(ILogger<HomeController> logger, DbCapsContext context )
        {
            this.logger = logger;
            this.contexto = context;
        }

        public async Task<IActionResult> Index()
        {
            var Rol = User.FindFirst(ClaimTypes.Role)?.Value;
            if (Rol == "ADMINISTRADOR") return RedirectToAction("Admin");
            else if (Rol == "EMPLEADO") return RedirectToAction("Emp");
            else return RedirectToAction("Login", "Home");
        }

        #region Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            //LoginDTO objeto = new LoginDTO()
            //{
            //    IdEmpleado = "ADM-000001",
            //    Contrasena = "12345"
            //};
            var serv_Login = new LoginService(contexto);
            int usuarioCorrecto = await serv_Login.VerificarCredenciales(loginDTO);
            if (usuarioCorrecto == 1)
            {
                int UsuarioActivo = await serv_Login.VerificarUsuarioActivo(loginDTO.IdEmpleado);
                if (usuarioCorrecto == 1)
                {
                    ClaimsIdentity claimIdentity = await serv_Login.CredencialesEmpleado(loginDTO.IdEmpleado);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {AllowRefresh = true,/*IsPersistent =false; */};

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimIdentity),
                        properties);

                    var Rol = User.FindFirst(ClaimTypes.Role)?.Value;
                    if (Rol == "ADMINISTRADOR") return RedirectToAction("Admin");
                    else if (Rol == "EMPLEADO") return RedirectToAction("Emp");
                    //else return RedirectToAction("Login", "Home");

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        #endregion


        [Authorize(Roles = "ADMINISTRADOR")]
        public IActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "EMPLEADO")]
        public IActionResult Inicio()
        {

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
