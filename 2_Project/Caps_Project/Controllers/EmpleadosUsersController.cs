using AutoMapper;
using Caps_Project.DTOs;
using Caps_Project.DTOs.EmpleadoDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caps_Project.Controllers
{
    public class EmpleadosUsersController : Controller
    {
        private readonly DbCapsContext contexto;
        private readonly IMapper mapper;
        public EmpleadosUsersController(DbCapsContext context, IMapper mapper)
        {
            this.contexto = context;
            this.mapper = mapper;
        }
        // GET: UsuariosController
        public ActionResult Index()
        {

            return View();
        }

        // Ver la lista de los Empleados
        [HttpPost]
        public async Task<JsonResult> ListaEmpleados()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            PaginationDTO paginationDTO = new PaginationDTO()
            {
                pageSize = length != null ? Convert.ToInt32(length) : 0,
                skip = start != null ? Convert.ToInt32(start) : 0
            };

            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            var rawUsuarios = await Serv_Inventario.ListaEmpleados(paginationDTO);

            return Json(new
            {
                Draw = draw,
                RecordsTotal = rawUsuarios.paginationDTO.recordsTotal,
                RecordsFiltered = rawUsuarios.paginationDTO.recordsTotal,
                Data = rawUsuarios.listUsuarios
            }); ;
        }
        [HttpGet]
        public IActionResult AgregarEmpleado()
        {
            return View();
        }
		// Agregar a un empleado
		[HttpPost]
        public async Task<IActionResult> AgregarEmpleado(InsertEmpleadoDTO empleadoDTO)
        {
            if (!ModelState.IsValid) { TempData["Error"] = "Invalid format"; return View("RegistraNuevoProducto", empleadoDTO); }

            // Registrar mi Empleado
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            string IdEmpleado = await Serv_Inventario.InsertarEmpleado(empleadoDTO);

            // DTO de Activar usuario
            ActivarUsuarioDTO activarUsuario = new ActivarUsuarioDTO() { IdEmpleado = IdEmpleado, Turno = empleadoDTO.Turno };
            //Activarlo
            bool IsActivated = await Serv_Inventario.ActivarUsuario(activarUsuario);

            if(!IsActivated) {
                TempData["Error"] = "Invalid format"; return View("RegistraNuevoProducto");
            }

            return RedirectToAction("index");
        }

        // Activar o desactivar el usuario
        public async Task<ActionResult> DeActivarEmpleado(string IdEmpleado)
        {
            ActivarUsuarioDTO usuario = new ActivarUsuarioDTO();
            usuario.IdEmpleado = IdEmpleado;
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            //Activarlo
            bool IsActivated = await Serv_Inventario.DesactivarUsuario(usuario.IdEmpleado);
            if(!IsActivated)
            {
				return NotFound();
			}
			return Ok(new { success = true });
		}

		// Obtener la lista de los turnos de los empleados
		public async Task<IActionResult> GetListaTipoEmp()
        {
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            var listaEmpleados = await Serv_Inventario.List_TipoEmpleado();

            return Json(listaEmpleados);
        }
        public async Task<IActionResult> GetListaTurnos()
        {
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            var ListaDeTurnos = await Serv_Inventario.ObtenerListaDeTurnos();

            return Json(ListaDeTurnos);
        }
    }
}
