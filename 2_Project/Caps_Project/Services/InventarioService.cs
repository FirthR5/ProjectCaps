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
        public async Task InsertProductCategory(ProductCategory categoryModel)
        {
            //context.ProductCategories.AsNoTracking();
            context.Add(categoryModel);
            context.SaveChanges();
        }
        public async Task<bool> ProductExists(int IdProducto)
        {
            bool count = context.Productos
                                .Any(p => p.IdProducto == 1);
            return count;
        }
        private void RegistrarProducto(Producto nuevoProducto)
        {
            context.Productos.Add(nuevoProducto);
            context.SaveChanges();
        }
        private void InsertOrUpdateProductPrices(NewProductoPriceDTO newProductPrice)
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductID", SqlDbType.Int) { Value = newProductPrice.ProductId },
                new SqlParameter("@UnitPrice", SqlDbType.Money) { Value = newProductPrice.UnitPrice }
            };

            context.Database.ExecuteSqlRaw("EXEC sp_InsertOrUpdateProductPrices @ProductID, @UnitPrice", parameters);
        }
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

    }
}
