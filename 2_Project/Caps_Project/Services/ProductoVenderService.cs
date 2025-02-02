﻿using AutoMapper;
using Azure.Identity;
using Caps_Project.DTOs;
using Caps_Project.DTOs.InventarioDTOs;
using Caps_Project.DTOs.OrdenDTOs;
using Caps_Project.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace Caps_Project.Services
{
    public class ProductoVenderService : BaseService
    {
        public ProductoVenderService(DbCapsContext context) : base(context) { }
        public ProductoVenderService(DbCapsContext context, IMapper mapper) : base(context, mapper) { }


        /// <summary>
        /// Lista de Productos
        /// </summary>
        /// <returns>List<Producto></returns>
        public async Task</*PaginationProductoDTO*/List<ProductDTO>> ListProductos(/*PaginationDTO paginationDTO*/)
        {
            var query = context.Productos.Where(p => p.Activo == true);

			//paginationDTO.recordsTotal = await query.CountAsync();
			//PaginationProductoDTO paginationProductoDTO = new PaginationProductoDTO()
			//{
			//    paginationDTO = paginationDTO
			//};

			//paginationProductoDTO.listProductos = await query.Skip(paginationDTO.skip)
			//    .Take(paginationDTO.pageSize).ToListAsync();

			var products = await query.ToListAsync();
			var productDTOs = new List<ProductDTO>();

			foreach (var product in products)
			{
				var unitPrice = await context.ProductPrices
											.Where(pp => pp.ProductId == product.IdProducto && pp.EndDate == null)
											.Select(p => p.UnitPrice)
											.FirstOrDefaultAsync();

				productDTOs.Add(new ProductDTO
				{
					IdProducto = product.IdProducto,
					ProductName = product.ProdName,
					Stock = product.Stock,
					UnitPrice = unitPrice
				});
			}

			return productDTOs;
			//return paginationProductoDTO;
		}

        /// <summary>
        ///  Desactivar producto del inventario por ID
        /// (Llamado por InventarioController)
        /// </summary>
        /// <param name="IdProducto"></param>
        /// <returns></returns>
        public async Task<bool> DesactivarProducto(int IdProducto)
        {
            var producto = context.Productos.FirstOrDefault(p => p.IdProducto == IdProducto);

            if (producto != null)
            {
                producto.Activo = false;
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }


        #region Carrito Temporal
        // TODO: Checar lo del UUID
        /// <summary>
        /// Agregar al Carrito
        /// </summary>
        /// <param name="carritoTmp"></param>
        /// <returns></returns>
        public async Task<bool> AgregarAlCarrito(ItemCarritoDTO itemCarritoDTO)
        {
            try
            {
                TempCarrito nuevoItem = mapper.Map<TempCarrito>(itemCarritoDTO);
                
                var productPrice = await context.ProductPrices
                                    .FirstAsync(p => p.EndDate == null && p.ProductId == itemCarritoDTO.ProductId);
                nuevoItem.ProductPriceId = productPrice.IdPrice;

                context.TempCarritos.Add(nuevoItem);
                await context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
           
            return true;
        }


        //public async Task</*PaginationProductoDTO*/List<TempCarrito>> ListTempCarrito(/*PaginationDTO paginationDTO*/)
        //{
        //    var query = context.TempCarritos;

        //    //paginationDTO.recordsTotal = await query.CountAsync();
        //    //PaginationProductoDTO paginationProductoDTO = new PaginationProductoDTO()
        //    //{
        //    //    paginationDTO = paginationDTO
        //    //};

        //    //paginationProductoDTO.listProductos = await query.Skip(paginationDTO.skip)
        //    //    .Take(paginationDTO.pageSize).ToListAsync();

        //    List<TempCarrito> s = query.ToList();
        //    return s;
        //    //return paginationProductoDTO;
        //}

        public async Task<List<ProductCarritoDTO>> VerCarritoItems()
        {
            try
            {
                // TODO: AutoMappers OrderReceiptDT

                var productItems = await context.ProductCarritoDTOs
                    .FromSqlRaw(@"
                             SELECT 
                                  IdItem, it.ProductId,
                                  (SELECT ProdName FROM Productos WHERE IdProducto = it.ProductID) AS ProductName, 
                                  Quantity, 
                                  UnitPrice, 
                                  ProductPriceID 
                              FROM TempCarrito it
                              INNER JOIN ProductPrices pp ON it.ProductPriceID = pp.IdPrice
                            ")
                    .ToListAsync();

                return productItems;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Eliminar un Item del carrito de compras temporal
        /// Mediante Id
        /// </summary>
        /// <param name="IdItem"></param>
        /// <returns></returns>
        public async Task<bool> EliminarItemDelCarrito(int IdItem)
        {
            try
            {
                var objeto = new TempCarrito { IdItem = IdItem };
                context.TempCarritos.Attach(objeto);
                context.TempCarritos.Remove(objeto);

                await context.SaveChangesAsync();
                return true;

            }
            catch { return false; }
        }

        // TODO: Revisar esto ya cuando tenga mi FrontEnd
        /// <summary>
        /// Editar cantidad de productos que ha escogido
        /// </summary>
        /// <param name="itemCarritoDTO"></param>
        /// <returns></returns>
        public async Task<bool> EditarItemCarrito(EditarCarritoDTO itemCarritoDTO)
        {
            try
            {
                var itemEditar = await context.TempCarritos
                                .FirstOrDefaultAsync(m => m.IdItem == itemCarritoDTO.IdItem);//TODO: OrderUUID si es que lo utilizo
                itemEditar.Quantity = itemCarritoDTO.Quantity;
                //nuevoItem = mapper.Map<TempCarrito>(itemCarritoDTO);

                context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // TODO: Evaluar despues si es bueno lo del UUID o Eliminar todos
        /// <summary>
        /// Eliminar todos los articulos del temporal
        /// </summary>
        /// <param name="orderUUID"></param>
        /// <returns></returns>
        public async Task<int> EliminarItemsTodos()//PorOrderUUID(Guid orderUUID)
        {
            var itemsAEliminar = await context.TempCarritos.ToListAsync();
            //var itemsAEliminar = await context.TempCarritos.Where(tc => tc.OrderUuid == orderUUID).ToListAsync();
            context.TempCarritos.RemoveRange(itemsAEliminar);
            return await context.SaveChangesAsync();
        }
        #endregion

        #region Carrito Pagar
        // TODO: Try-Catch
        // TODO: Ver si funciona
        private async Task<decimal> TotalPagar(List<TempCarrito> productItems)
        {
            decimal total = 0;  

            // TODO: Saltarse lo que tienen 0 quantity items
            // Get: Lista de todos los productos del carrito temporal
            var ItemsCarritoTmp = await context.TempCarritos
                .Select(m => new {m.IdItem, m.ProductPriceId}).ToListAsync();
            // Obtener precio PrecioTotal
            foreach (var precio in ItemsCarritoTmp){
                total += context.ProductPrices.First(p => p.IdPrice == precio.ProductPriceId).UnitPrice;
            }
            return total;
        }

                
        // TODO: Evaluar si es mejor sacar los Items de la BD
        public async Task<int> RegistrarOrden(string idEmpleado, List<TempCarrito> tempProdItems )
        {
            // TODO: Funcionalidad
           
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Obtener precio PrecioTotal
                    // Asignar precio total
                    decimal totalPaid = await TotalPagar(tempProdItems);

                    // Crear el OrderReceipt
                    var orderReceipt = new OrderReceipt
                    {
                        IdEmpleado = idEmpleado,
                        TotalPaid = totalPaid,
                        OrderDate = DateTime.Now,
                    };
                    context.OrderReceipts.AddAsync(orderReceipt);
                    await context.SaveChangesAsync();

                    // =========================================================
                    // Get: Lista de todos los productos del carrito temporal
                    // =========================================================
                    //var ItemsCarritoTmp = await context.TempCarritos.ToListAsync();

                    // Registrar productos Items (IdOrden a Items)
                    // Asignar el OrderId generado a los Product_Items
                    // TODO: Saltarse lo que tienen 0 quantity items
                    List<ProductItem> ProdItems = mapper.Map<List<ProductItem>>(tempProdItems);
                    foreach (var item in ProdItems){
                        item.IdItem = 0;
                        item.TicketOrderId = orderReceipt.OrderId;
                    }

                    // Insertar los Product_Items
                    await context.ProductItems.AddRangeAsync(ProdItems);
                    await context.SaveChangesAsync();

                    // Eliminar todo los items del carrito
                    int correcto = await EliminarItemsTodos();
                    if (correcto <1){
                        transaction.Rollback();
                        return 0;
                    }

                    // TODO: Check this later, i think and i remember the Trigger will no work with this.

                    // Commmit the Transaction
                    transaction.Commit();
                    return orderReceipt.OrderId; 
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                    //throw new ApplicationException("Error al crear el OrderReceipt y sus items", ex);
                }
            }
           
        }
        #endregion

        // TODO: Try-Catch
        public async Task<List<OrderReceipt>> VerOrdenesRealizadas(string idEmpleado)
        {
            try
            {
                // TODO: AutoMappers OrderReceiptDT
                var query = context.OrderReceipts.Where(o => o.IdEmpleado == idEmpleado);
                var ordenes = await query.ToListAsync();
                return ordenes;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ProductItemDTO>> VerOrderItems(int OrderId)
        {
            try
            {
                // TODO: AutoMappers OrderReceiptDT
                var orderIdParameter = new SqlParameter("@OrderId", OrderId);

                var productItems = await context.ProductItemsDTOs
                    .FromSqlRaw(@"
                            SELECT 
                                IdItem, 
                                (SELECT ProdName FROM Productos WHERE IdProducto = it.ProductID) AS ProductName, 
                                Quantity, 
                                UnitPrice, 
                                ProductPriceID
                            FROM Product_Items it
                            LEFT JOIN ProductPrices pp ON it.ProductID = pp.ProductID
                            WHERE OrderId = @OrderId", orderIdParameter)
                    .ToListAsync();

                return productItems;
            }
            catch
            {
                return null;
            }
        }



    }
}
