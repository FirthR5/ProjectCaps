using Caps_Project.Models;

namespace Caps_Project.DTOs
{
    public class PaginationDTO
    {
        public int pageSize { get; set; } = 0;
        public int skip { get; set; } = 0;
        public int recordsTotal { get; set; } = 0;
    }
    public class PaginationProductoDTO
    {
        public PaginationDTO paginationDTO { get; set; }
        public List<Producto> listProductos { get; set; }
    }
    public class PaginationUsuarioDTO
    {
        public PaginationDTO paginationDTO { get; set; }
        public List<VwDatosUsuario> listUsuarios { get; set; }
    }
}
