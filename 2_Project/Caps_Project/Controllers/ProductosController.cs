using AutoMapper;
using Caps_Project.DTOs.EmpleadoDTOs;
using Caps_Project.DTOs.OrdenDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caps_Project.Controllers
{
    public class ProductosController : Controller
    {
        // GET: ProductosController
        private readonly DbCapsContext contexto;
        private readonly IMapper mapper;
        public ProductosController(DbCapsContext context, IMapper mapper)
        {
            this.contexto = context;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Carrito Item
        /// <summary>
        /// API Agregar al Carrito
        /// </summary>
        /// <param name="carritoTmp"></param>
        /// <returns></returns>
        public async Task AgregarAlCarrito([FromBody] ItemCarritoDTO itemCarritoDTO)
        {
            //TODO: Crear el UUID
            // carritoTmp.OrderUuid = ???
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            bool ejecucionCorrecta = await Serv_Inventario.AgregarAlCarrito(itemCarritoDTO);
            // TODO: agregar
            //ejecucionCorrecta
        }
        // TODO: Crear metodo de Editar 
        /// <summary>
        /// Actualizar la cantidad de items seleccionados del Carrito temporal
        /// </summary>
        /// <param name="editarCarrito"></param>
        /// <returns></returns>
        public async Task EditarItemCarrito([FromBody] EditarCarritoDTO editarCarrito )
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            bool IsActivated = await Serv_Inventario.EditarItemCarrito(editarCarrito);
            // return ???;
        }
        /// <summary>
        /// Eliminar un item del carrito temporal 
        /// </summary>
        /// <param name="IdItemTemp"></param>
        /// <returns></returns>
        public async Task EliminarItemDelCarrito(int IdItemTemp)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            //Activarlo
            bool IsActivated = await Serv_Inventario.EliminarItemDelCarrito(IdItemTemp);
            //return ???;
        }
       #endregion

        #region Orden
        /// <summary>
        /// Realiza ya la accion de Registrar la Orden en el sistema
        /// Esta parte de encarga de crear la orden, registrar los productos del carrito temporal en el sistema (tbl product items)
        /// y recibir el id de la orden una vez creada
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        private async Task RegistrarOrden(string idEmpleado)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            var productosItems = contexto.ProductItems.ToList();
            int OrderId = await Serv_Inventario.RegistrarOrden(idEmpleado, productosItems );

        }
        /// <summary>
        /// Ver lista de Ordenes realizados por el mismo usuario de la sesion actual
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        private async Task VerOrdenesRealizadas(string idEmpleado)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            var listaOrdenes_User = await Serv_Inventario.VerOrdenesRealizadas(idEmpleado);
            // return ???;
        }
        /// <summary>
        /// Ver Items de una Orden especifica en base a su ID Orden
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        private async Task VerOrderItems(int OrderId)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            var listaItemsOfOrden = await Serv_Inventario.VerOrderItems(OrderId);
            // return ???;
        }
        
        #endregion

    }
}
