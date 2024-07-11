using AutoMapper;
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

            // SourceModel, DestinationDTO
            CreateMap<Empleado, ActivarUsuarioDTO>();
            CreateMap<OrderReceipt, OrderReceiptDTO>();
            //CreateMap<ProductItem, >();
            CreateMap<Empleado, LoginDTO>();
            CreateMap<Empleado, InsertEmpleadoDTO>();

            //CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            //CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
