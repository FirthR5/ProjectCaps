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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            ProductoVenderService d = new ProductoVenderService(contexto);
            List<ProductDTO> x = await d.ListProductos();
            return View(x);
        }

        #region Carrito Item
        /// <summary>
        /// API Agregar al Carrito
        /// </summary>
        /// <param name="carritoTmp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AgregarAlCarrito([FromBody] ItemCarritoDTO itemCarritoDTO)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto, mapper);
            bool ejecucionCorrecta = await Serv_Inventario.AgregarAlCarrito(itemCarritoDTO);

            if (!ejecucionCorrecta) {
                return BadRequest(new {message = "No se pudo guardar en el carrito el producto"});
            }
            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> CarritoVenta()
        {

            ProductoVenderService d = new ProductoVenderService(contexto);
            List<ProductCarritoDTO> x = await d.VerCarritoItems();

            //List<TempCarrito> x = await d.ListTempCarrito();
            return View(x);
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
        [HttpGet]
        public async Task<IActionResult> RegistrarOrden()
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto, mapper);
            string idEmpleado  = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var productosItems = contexto.TempCarritos.ToList();
            if (productosItems.Count != 0)
            {
                int OrderId = await Serv_Inventario.RegistrarOrden(idEmpleado, productosItems);
            }
            return RedirectToAction("index", "Home");

        }
        /// <summary>
        /// Ver lista de Ordenes realizados por el mismo usuario de la sesion actual
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        [HttpGet]
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
        [HttpGet]
        private async Task VerOrderItems(int OrderId)
        {
            ProductoVenderService Serv_Inventario = new ProductoVenderService(contexto);
            var listaItemsOfOrden = await Serv_Inventario.VerOrderItems(OrderId);
            // return ???;
        }
        
        #endregion

    }
}
