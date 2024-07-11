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
            var listaProductos = await Serv_Inventario.ListProductos();

            return View(listaProductos);
        }

        // Registrar un Producto
        // TODO: Agregar redirecciones
        /// <summary>
        /// Registrar productos
        /// </summary>
        /// <param name="nuevoProducto"></param>
        /// <returns></returns>
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
        public async Task<ActionResult> AlmacenaProducto([FromBody]NewProductoPriceDTO newProductPrice)
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
            }//
            catch
            {
                return View();
            }
        }

        // Actualiza Precio de Producto
        /// <summary>
        /// Actualizar el precio del producto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarPrecio(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // Deshabilita Producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableProduct(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // ????
        public ActionResult Details(int id)
        {
            return View();
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
