using System.ComponentModel.DataAnnotations;

namespace Caps_Project.DTOs.InventarioDTOs
{
	public class ProductoDTO
	{

		[Key]
		public int IdProducto { get; set; }

		public string ProdName { get; set; } = null!;

		public int Stock { get; set; }

		public int IdProdCategory { get; set; }

		public string? Descripcion { get; set; }

		public bool Activo { get; set; }
	}
}
