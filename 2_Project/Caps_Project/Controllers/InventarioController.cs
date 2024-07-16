using AutoMapper;
using Azure.Identity;
using Caps_Project.DTOs;
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
        [HttpPost]
        public async Task<JsonResult> ListaInventario()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            PaginationDTO paginationDTO = new PaginationDTO(){
                pageSize = length != null ? Convert.ToInt32(length) : 0,
                skip = start != null ? Convert.ToInt32(start) : 0
            };
            
            InventarioService Serv_Inventario = new InventarioService(contexto);
            var rawProductos = await Serv_Inventario.List_Productos(paginationDTO);
            var listaProductos = mapper.Map<List<ProductoDTO>>(rawProductos.listProductos);

            return Json(new
            {
                Draw = draw,
                RecordsTotal = rawProductos.paginationDTO.recordsTotal,
                RecordsFiltered = rawProductos.paginationDTO.recordsTotal,
                Data = listaProductos
            });
        }


        [HttpGet]
        public IActionResult RegistraNuevoProducto()
        {

            return View();
        }

        // Registrar un Producto
        /// <summary>
        /// Registrar productos
        /// </summary>
        /// <param name="nuevoProducto"></param>
        /// <returns></returns>
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistraNuevoProducto([FromForm] NuevoProductoDTO nuevoProducto)
        {

            try
            {
                if(!ModelState.IsValid) { TempData["Error"] = "Invalid format"; return View("RegistraNuevoProducto", nuevoProducto); }
                // Productos + Product Prices
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool queryExitoso = await Serv_Inv.RegistrarProducto(nuevoProducto);
                if (!queryExitoso)
                {
                    TempData["Error"] = "Invalid format"; return View("RegistraNuevoProducto");
                }

                return RedirectToAction("index");
            }
            catch
            {
                    TempData["Error"] = "Invalid format"; return View("RegistraNuevoProducto");
            }
        }

        [HttpGet]
        public ActionResult ActualizarPrecio()
        {
            // Metodo de prubea para ver el Modal
            return View();
        }
        // Almacenar Producto
        [HttpPost]
		public async Task<ActionResult> AlmacenaProducto([FromBody] IngresoInventarioDTO ingresoInventario)
		{
            try
            {
                // Productos + Product Prices
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
				var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

				bool queryExitoso = await Serv_Inv.IngresoProdInventario(ingresoInventario, userId);

                if (!queryExitoso)
                {
					return NotFound();
				}

				return Ok(new { success = true });
			}
            catch
            {
				return NotFound();
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
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ActualizarPrecio([FromBody] NewProductoPriceDTO newProductPrice)
        {
            try
            {
                InventarioService Serv_Inv = new InventarioService(contexto, mapper);
                bool executionSuccessful = await Serv_Inv.InsertOrUpdateProductPrices(newProductPrice);

                if (!executionSuccessful)
                {
					return NotFound();
				}
				return Ok(new { success = true });
			}
			catch
            {
				return NotFound();
			}
		}
        /// <summary>
        // Deshabilita el Producto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableProduct(int id)
        {
            try
            {
				ProductoVenderService Serv_Inv = new ProductoVenderService(contexto);
                bool executionSuccessful = await Serv_Inv.DesactivarProducto(id);
                if (!executionSuccessful)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Obtener la lista de la categoria de los productos
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetListProdCat()
        {
            InventarioService Serv_Inv = new InventarioService(contexto);
            List<ProductCategory> lista = await Serv_Inv.ListProductCategorias();
            var listaCategorias = mapper.Map<List<ProdCategoryDTO>>(lista);
            return Json(listaCategorias);
        }
    }
}
