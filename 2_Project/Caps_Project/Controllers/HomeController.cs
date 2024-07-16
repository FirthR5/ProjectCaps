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
            if (Rol == "ADMINISTRADOR") return RedirectToAction("Dashboard");
            else if (Rol == "EMPLEADO") return RedirectToAction("Inicio");
            else return RedirectToAction("Login", "Home");
        }

        #region Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var Rol = User.FindFirst(ClaimTypes.Role)?.Value;
            if (Rol == "ADMINISTRADOR") return RedirectToAction("Dashboard");
            else if (Rol == "EMPLEADO") return RedirectToAction("Inicio");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login( LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) { TempData["Error"] = "Invalid format"; return View("Login", loginDTO); }

            var serv_Login = new LoginService(contexto);


            int usuarioCorrecto = await serv_Login.VerificarCredenciales(loginDTO);
            if (usuarioCorrecto == 1)
            {
                int UsuarioActivo = await serv_Login.VerificarUsuarioActivo(loginDTO.UserName);
                if (UsuarioActivo == 1)
                {
                    ClaimsIdentity claimIdentity = await serv_Login.CredencialesEmpleado(loginDTO.UserName);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {AllowRefresh = true,/*IsPersistent =false; */};

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimIdentity),
                        properties);

                    //var Rol = User.FindFirst(ClaimTypes.Role)?.Value;
                    var Rol = claimIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    if (Rol == "ADMINISTRADOR") return RedirectToAction("Dashboard");
                    else if (Rol == "EMPLEADO") return RedirectToAction("Inicio");
                    //else return RedirectToAction("Login", "Home");

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "El usuario no se encuentra activo.";
                    return View("Login", loginDTO);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Credenciales Incorrectas.";

                return View("Login", loginDTO);
            }
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        #endregion


        [HttpGet]
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
