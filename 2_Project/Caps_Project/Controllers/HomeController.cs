using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Caps_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        private readonly DbCapsContext _contexto;
        public HomeController(ILogger<HomeController> logger, DbCapsContext context )
        {
            _logger = logger;
            _contexto = context;
        }

        public async Task<IActionResult> Index()
        {
            LoginDTO objeto = new LoginDTO()
            {
                IdEmpleado= "ADM-000001",
                Contrasena = "12345"
            };
            var obj = new LoginService(_contexto);
            int exitoso = await obj.VerificarCredenciales(objeto);
            return View();
        }

        public IActionResult Privacy()
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
