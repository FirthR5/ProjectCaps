using AutoMapper;
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

        // TODO: Hacer Obtener Lista de Empleados
        // Ver la lista de los Empleados
        private async Task<ActionResult> ListaEmpleados()
        {
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            var listaEmpleados = await Serv_Inventario.ListaEmpleados();

            return View(listaEmpleados);
        }
        // Agregar a un empleado
        private async void AgregarEmpleado(InsertEmpleadoDTO empleadoDTO)
        {
            // Registrar mi Empleado
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            string IdEmpleado = await Serv_Inventario.InsertarEmpleado(empleadoDTO);

            // DTO de Activar usuario
            ActivarUsuarioDTO activarUsuario = new ActivarUsuarioDTO() { IdEmpleado = IdEmpleado, Turno = empleadoDTO.Turno };
            //Activarlo
            bool IsActivated = await Serv_Inventario.ActivarUsuario(activarUsuario);

            //return View(listaProductos);
        }

        // Activar o desactivar el usuario
        private async void DeActivarEmpleado(string IdEmpleado)
        {
            // TODO: Agregar DTO to DTO
            ActivarUsuarioDTO usuario = new ActivarUsuarioDTO();
            usuario.IdEmpleado = IdEmpleado;
            EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
            //Activarlo
            bool IsActivated = await Serv_Inventario.ActivarUsuario(usuario);
        }

        // TODO: Ver si esto se implementa o neh
        // Obtener la lista de los turnos de los empleados
        //private void GetListaTurnos()
        //{
        //    EmpleadoService Serv_Inventario = new EmpleadoService(contexto);
        //    var listaEmpleados = await Serv_Inventario.List_TipoEmpleado();

        //    return View(listaProductos);
        //}

    }
}
