using AutoMapper;
using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caps_Project.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbCapsContext contexto;
        private readonly IMapper mapper;
        public LoginController(DbCapsContext context, IMapper mapper)
        {
            this.contexto = context;
            this.mapper = mapper;
        }
        // GET: LoginController

    }
}
