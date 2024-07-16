using System.ComponentModel.DataAnnotations;

namespace Caps_Project.DTOs.OrdenDTOs
{
    public class ProductDTO
    {
        [Key]
        public int IdProducto { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }


        public int Stock { get; set; }

    }
}
