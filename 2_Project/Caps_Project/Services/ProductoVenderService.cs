using Azure.Identity;
using Caps_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Caps_Project.Services
{
    public class ProductoVenderService : BaseService
    {
        public ProductoVenderService(DbCapsContext context) : base(context) { }
        private void DesactivarProducto(int IdProducto)
        {
            var producto = context.Productos.FirstOrDefault(p => p.IdProducto == IdProducto);

            if (producto != null)
            {
                producto.Activo = false;
                context.SaveChangesAsync();
            }
            else
            {
                // Manejar el caso en que el producto no exista, si es necesario
                throw new Exception($"No se encontró el producto con IdProducto = {IdProducto}");
            }
        }
        private void AgregarAlCarrito(TempCarrito carritoTmp)
        {
            context.TempCarritos.Add(carritoTmp);
            context.SaveChangesAsync();
        }
        private void EliminarUnDelCarrito(TempCarrito carritoTmp)
        {
            context.TempCarritos.Add(carritoTmp);
            context.SaveChangesAsync();
        }
        public async Task<int> EliminarItemsPorOrderUUID(Guid orderUUID)
        {
            var itemsAEliminar = await context.TempCarritos.Where(tc => tc.OrderUuid == orderUUID).ToListAsync();
            context.TempCarritos.RemoveRange(itemsAEliminar);
            return await context.SaveChangesAsync();
        }
        private decimal TotalPagar(List<ProductItem> productItems)
        {
            // TODO: Funcionalidad
            return 0;
        }
        public async Task<int> RegistrarOrden(string idEmpleado, List<ProductItem> productItems )
        {
            // TODO: Check this later, i think and i remember the Trigger will no work with this.
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Crear el OrderReceipt
                    decimal totalPaid = TotalPagar(productItems);
                    var orderReceipt = new OrderReceipt
                    {
                        IdEmpleado = idEmpleado,
                        TotalPaid = totalPaid,
                        OrderDate = DateTime.Now,
                    };
                    context.OrderReceipts.AddAsync(orderReceipt);
                    await context.SaveChangesAsync();

                    // Asignar el OrderId generado a los Product_Items
                    foreach (var item in productItems)
                    {
                        item.TicketOrderId = orderReceipt.OrderId;
                    }

                    // Insertar los Product_Items
                    await context.ProductItems.AddRangeAsync(productItems);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                    return orderReceipt.OrderId; // Retornar el Id del OrderReceipt creado
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ApplicationException("Error al crear el OrderReceipt y sus items", ex);
                }
            }
           
        }

        public async Task<List<OrderReceipt>> VerOrdenesRealizadas(string idEmpleado)
        {
            // TODO: AutoMappers OrderReceiptDT
            var query = context.OrderReceipts.Where(o => o.IdEmpleado == idEmpleado);
            var ordenes = await query.ToListAsync();
            return ordenes;
        }
        public async Task<List<ProductItem>> VerOrderItems(int OrderId)
        {
            // TODO: AutoMappers OrderReceiptDT
            var query = context.ProductItems.Where(o => o.TicketOrderId == OrderId);
            var ordenes = await query.ToListAsync();
            return ordenes;
        }
    }
}
