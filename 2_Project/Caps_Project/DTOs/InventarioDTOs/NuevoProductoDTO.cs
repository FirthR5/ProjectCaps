using System.ComponentModel.DataAnnotations;

namespace Caps_Project.DTOs.InventarioDTOs
{
    public class NuevoProductoDTO
    {
        // Product
        [Key]
        public string ProdName { get; set; } = null!;

        public int Stock { get; set; }

        public int IdProdCategory { get; set; }

        public string? Descripcion { get; set; }

        public bool Activo { get; set; }

        // ProductPrice
        public decimal UnitPrice { get; set; }

    }
}
