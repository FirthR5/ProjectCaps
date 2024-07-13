using AutoMapper;
using Caps_Project.DTOs.InventarioDTOs;
using Caps_Project.Models;
using Caps_Project.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caps_Project.Controllers
{
    public class InventarioController : Controller
    {
        private readonly DbCapsContext contexto;
        private readonly IMapper mapper;
        public InventarioController(DbCapsContext context, IMapper mapper)
        {
            this.contexto = context;
            this.mapper = mapper;
        }

        // GET: InventarioController
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Obtener lista de los productos activos del inventario
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ListaInventario()
        {
            InventarioService Serv_Inventario = new InventarioService(contexto);
            var listaProductos = await Serv_Inventario.List_TipoEmpleado();

            return View(listaProductos);
        }
        [HttpGet]
        public ActionResult RegistraNuevoProducto()
        {
            return View();
        }
        // Registrar un Producto
        // TODO: Agregar redirecciones
        /// <summary>
        /// Registrar productos
        /// </summary>
        /// <param name="nuevoProducto"></param>
        /// <returns></returns>
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistraNuevoProducto([FromBody] NuevoProductoDTO nuevoProducto)
        {

            try
            {
                // Productos + Product Prices
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool queryExitoso = await Serv_Inv.RegistrarProducto(nuevoProducto);
                if ( !queryExitoso)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Almacenar Producto
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> AlmacenaProducto(IngresoInventarioDTO ingresoInventario)
        {
            try
            {
                // Productos + Product Prices
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool queryExitoso = await Serv_Inv.IngresoProdInventario(ingresoInventario);

                if (!queryExitoso)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// TODO: Agregar redirecciones
        // Actualiza Precio de Producto
        /// <summary>
        /// Actualizar el precio del producto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ActualizarPrecio([FromBody]NewProductoPriceDTO newProductPrice)
        {
            try
            {
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool executionSuccessful = await Serv_Inv.InsertOrUpdateProductPrices(newProductPrice);
                if (!executionSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        // Deshabilita el Producto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableProduct(int id)
        {
            try
            {
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool executionSuccessful = await Serv_Inv.DesactivarProducto(id);
                if (!executionSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> GetListProdCat()
        {
            InventarioService Serv_Inv = new InventarioService(contexto);
            List<ProductCategory> lista = await Serv_Inv.ListProductCategorias();
            var listaCategorias = mapper.Map<List<ProdCategoryDTO>>(lista);
            return Ok(listaCategorias);
        }
    }
}
