﻿using AutoMapper;
using Caps_Project.Models;
using Caps_Project.DTOs;
using Caps_Project.DTOs.EmpleadoDTOs;
using Caps_Project.DTOs.InventarioDTOs;
using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.DTOs.OrdenDTOs;

namespace Caps_Project.DTOs
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductCategory, ProdCategoryDTO>().ReverseMap();

            CreateMap<NuevoProductoDTO, Producto>()
                .ForMember(name => name.ProdName,
                          opt => opt.MapFrom(src => src.ProdName))
                .ForMember(name => name.Stock,
                          opt => opt.MapFrom(src => src.Stock))
                .ForMember(name => name.IdProdCategory,
                          opt => opt.MapFrom(src => src.IdProdCategory))
                .ForMember(name => name.Descripcion,
                          opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(name => name.Activo,
                          opt => opt.MapFrom(src => src.Activo))
                ;

            CreateMap<NuevoProductoDTO, ProductPrice>()
                .ForMember(name => name.UnitPrice,
                          opt => opt.MapFrom(src => src.UnitPrice))
                                              ;
            CreateMap<IngresoInventarioDTO, Inventario>()
                .ForMember(name => name.IdProduct,
                          opt => opt.MapFrom(src => src.IdProduct))
                 .ForMember(name => name.Quantity,
                          opt => opt.MapFrom(src => src.Quantity))
                  //.ForMember(name => name.IdAdmin,
                  //        opt => opt.MapFrom(src => src.IdAdmin))
                  ;
            CreateMap<EditarProductoDTO, Producto>();
            CreateMap<EditarProductoDTO, Producto>().ReverseMap();

            CreateMap<ItemCarritoDTO, TempCarrito>()
               .ForMember(name => name.ProductId,
                         opt => opt.MapFrom(src => src.ProductId))
                .ForMember(name => name.Quantity,
                         opt => opt.MapFrom(src => src.Quantity))
                ;

            CreateMap<TempCarrito, ProductItem>()
               .ForMember(name => name.ProductId,
                         opt => opt.MapFrom(src => src.ProductId))
                .ForMember(name => name.Quantity,
                         opt => opt.MapFrom(src => src.Quantity))
                .ForMember(name => name.ProductPriceId,
                         opt => opt.MapFrom(src => src.ProductPriceId))
                ;


            CreateMap<Producto, ProductoDTO>();

            // SourceModel, DestinationDTO
            CreateMap<Empleado, ActivarUsuarioDTO>();
            CreateMap<OrderReceipt, OrderReceiptDTO>();
            //CreateMap<ProductItem, >();
            CreateMap<Empleado, InsertEmpleadoDTO>();

            //CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            //CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
