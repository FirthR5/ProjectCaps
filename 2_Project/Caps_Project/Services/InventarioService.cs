using AutoMapper;
using Caps_Project.DTOs;
using Caps_Project.DTOs.InventarioDTOs;
using Caps_Project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;

namespace Caps_Project.Services
{
    public class InventarioService : BaseService
    {
        public InventarioService(DbCapsContext context) : base(context) { }
        public InventarioService(DbCapsContext context, IMapper mapper) : base(context, mapper) { }

        /// <summary>
        /// Categoria de Productos
        /// </summary>
        /// <returns>Lista de Categorias de Productos</returns>
        public async Task<List<ProductCategory>> ListProductCategorias()
        {
            //Todo: Auto Encoder
            List<ProductCategory> listaCaegories = context.ProductCategories.ToList();
            return listaCaegories;
        }


        // TODO: Probar si funciona
        /// <summary>
        /// Obtener los Detalles Productos
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>
        private async Task<EditarProductoDTO> DetallesProducto(int idProducto)
        {
            Producto modelo = await context.Productos.FirstOrDefaultAsync(p => p.IdProducto == 1);
            EditarProductoDTO normal = mapper.Map<EditarProductoDTO>(modelo);
            return normal;
        }

        // TODO:  Ver si funciona
        private async Task<bool> EditarProducto(EditarProductoDTO product)
        {
            Producto normal = mapper.Map<Producto>(product);
            bool count = context.Productos.Any(p => p.IdProducto == 1);

            await context.SaveChangesAsync();

            return count;
        }

        // TODO: Agregarlo Despues
        private async Task InsertProductCategory(ProductCategory categoryModel)
        {
            //context.ProductCategories.AsNoTracking();
            context.Add(categoryModel);
            context.SaveChanges();
        }
        
        // TODO: Quitarle el Insert y poner el metodo (store procedure) que esta abajo de este metodo.
        /// <summary>
        /// Registrar Nuevos Productos
        /// </summary>
        /// <param name="nuevoProducto"></param>
        /// <returns></returns>
        public async Task<bool> RegistrarProducto(NuevoProductoDTO nuevoProducto)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Mapear estos objetos a su originales
                    Producto Producto = mapper.Map<Producto>(nuevoProducto);
                    ProductPrice Precio = mapper.Map<ProductPrice>(nuevoProducto);

                    // Crear/Registrar Nuevo Producto
                    Producto.Activo = true;
                    context.Productos.Add(Producto);
                    await context.SaveChangesAsync();

                    // Actualizar Precio del Producto
                    Precio.ProductId = Producto.IdProducto;
                    Precio.StartDate = DateTime.Now;
                    context.ProductPrices.Add(Precio);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        /// <summary>
        /// Poner el Precio de Productos
        /// Insertar Nuevo o Insert
        /// </summary>
        /// <param name="newProductPrice"></param>
        public async Task<bool> InsertOrUpdateProductPrices(NewProductoPriceDTO newProductPrice)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = newProductPrice.ProductId },
                    new SqlParameter("@UnitPrice", SqlDbType.Money) { Value = newProductPrice.UnitPrice }
                };
                int rowsAffected = await context.Database.ExecuteSqlRawAsync("EXEC sp_InsertOrUpdateProductPrices @ProductID, @UnitPrice", parameters);
                bool executionSuccessful = rowsAffected > 0;

                return executionSuccessful;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Ingreso de Nueva Cantidad de Productos al Inventario
        /// </summary>
        /// <param name="ingresoInventario"></param>
        public async Task<bool> IngresoProdInventario(IngresoInventarioDTO ingresoInventario)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Aplicar un automapper aqui
                    var nuevoInventario = mapper.Map<Inventario>(ingresoInventario);

					nuevoInventario.EntryDate = DateTime.Now;
					nuevoInventario.IdAdmin ="ADM-000001";
					context.Inventarios.Add(nuevoInventario);

                    await context.SaveChangesAsync();
					transaction.Commit();
					return true;
                }
                catch (Exception ex)
                {
					transaction.Rollback();
					return false;
                }
            }
        }
       
        /// <summary>
        /// Lista de Productos
        /// </summary>
        /// <returns>List<Producto></returns>
        public async Task<PaginationProductoDTO> List_TipoEmpleado(PaginationDTO paginationDTO)
        {
            var query = context.Productos;

            paginationDTO.recordsTotal = await query.CountAsync(); 
            PaginationProductoDTO paginationProductoDTO = new PaginationProductoDTO(){
                paginationDTO = paginationDTO
            } ;

            paginationProductoDTO.listProductos = await query.Skip(paginationDTO.skip)
                .Take(paginationDTO.pageSize).ToListAsync();



            return paginationProductoDTO;
        }
    }
}
