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

    }
}
