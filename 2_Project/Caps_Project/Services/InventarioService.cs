using AutoMapper;
using Caps_Project.DTOs.InventarioDTOs;
using Caps_Project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

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



        private async Task<bool> ProductExists(int IdProducto)
        {
            bool count = context.Productos.Any(p => p.IdProducto == 1);
            return count;
        }

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
        private void IngresoProdInventario(IngresoInventarioDTO ingresoInventario)
        {
            // Aplicar un automapper aqui
            var nuevoInventario = new Inventario
            {
                IdProduct = ingresoInventario.IdProduct,
                Quantity = ingresoInventario.Quantity,
                IdAdmin = ingresoInventario.IdAdmin
            };
            nuevoInventario.EntryDate = DateTime.Now;
            context.Inventarios.Add(nuevoInventario);
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Lista de Productos
        /// </summary>
        /// <returns>List<Producto></returns>
        public async Task<List<Producto>> ListProductos()
        {
            var query = context.Productos;
            List<Producto> list = await query.ToListAsync();
            return list;
        }
    }
}
